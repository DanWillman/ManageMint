#!/bin/bash

#credit to https://github.com/rafaelugolini/couchbase-server-nosetup https://www.couchbase.com/blog/using-docker-develop-couchbase/

# Monitor mode (used to attach into couchbase entrypoint)
set -m
# Send it to background
/entrypoint.sh couchbase-server &

# Check if couchbase server is up
check_db() {
  curl --silent http://127.0.0.1:8091/pools > /dev/null
  echo $?
}

# Variable used in echo
i=1
# Echo with
numbered_echo() {
  echo "[$i] $@"
  i=`expr $i + 1`
}

# Parse JSON and get nodes from the cluster
read_nodes() {
  cmd="import sys,json;"
  cmd="${cmd} print(','.join([node['otpNode']"
  cmd="${cmd} for node in json.load(sys.stdin)['nodes']"
  cmd="${cmd} ]))"
  python3 -c "${cmd}"
}

# Wait until it's ready
until [[ $(check_db) = 0 ]]; do
  >&2 numbered_echo "Waiting for Couchbase Server to be available"
  sleep 1
done

echo "# Couchbase Server Online"
echo "# Starting setup process"

HOSTNAME=`hostname -f`

# Reset steps
i=1
# Configure
numbered_echo "Initialize the node"
curl --silent "http://${HOSTNAME}:8091/nodes/self/controller/settings" \
  -d path="/opt/couchbase/var/lib/couchbase/data" \
  -d index_path="/opt/couchbase/var/lib/couchbase/data"

numbered_echo "Setting hostname"
curl --silent "http://${HOSTNAME}:8091/node/controller/rename" \
  -d hostname=${HOSTNAME}

if [[ ${CLUSTER_HOST} ]];then
  numbered_echo "Joining cluster ${CLUSTER_HOST}"
  curlCmd="-u {USERNAME}:${PASSWORD} http://${CLUSTER_HOST}:8091/controller/addNode -d hostname=\"${HOSTNAME}\" -d user=\"${USERNAME}\" -d password=\"${PASSWORD}\" -d services=\"${SERVICES}\""
  numbered_echo "curling with ${curlCmd}"
  curl ${curlCmd}

  if [[ ${CLUSTER_REBALANCE} ]]; then
    sleep 2
    curlCmd="-u ${USERNAME}:${PASSWORD} http://${CLUSTER_HOST}:8091/pools/default"
    numbered_echo "Retrieving nodes with ${curlCmd}"
    known_nodes=$(
      curl ${curlCmd} | read_nodes
    )

    numbered_echo "Known nodes: ${known_nodes}"

    curlCmd="-u ${USERNAME}:${PASSWORD} http://${CLUSTER_HOST}:8091/controller/rebalance -d knownNodes=\"${known_nodes}\""
    numbered_echo "Rebalancing cluster with curl ${curlCmd}"
    curl ${curlCmd}
  fi

else
  numbered_echo "Setting up memory"
  curl --silent "http://${HOSTNAME}:8091/pools/default" \
    -d memoryQuota=${MEMORY_QUOTA} \
    -d indexMemoryQuota=${INDEX_MEMORY_QUOTA} \
    -d ftsMemoryQuota=${FTS_MEMORY_QUOTA}

  numbered_echo "Setting up services"
  curl --silent "http://${HOSTNAME}:8091/node/controller/setupServices" \
    -d services="${SERVICES}"

  numbered_echo "Setting up user credentials"
  curl --silent "http://${HOSTNAME}:8091/settings/web" \
    -d port=8091 \
    -d username=${USERNAME} \
    -d password=${PASSWORD} > /dev/null

  numbered_echo "Creating buckets"
  couchbase-cli bucket-create --cluster "couchbase://${HOSTNAME}:8091" --username ${USERNAME} --password ${PASSWORD} --bucket sample --bucket-type couchbase --bucket-ramsize 128

  sleep 20

  numbered_echo "Importing data"
  cbimport json --cluster "couchbase://${HOSTNAME}:8091" --username ${USERNAME} --password ${PASSWORD} --bucket sample --dataset file://opt//couchbase//generated.json --format list --generate-key key::%name%::#UUID# --threads 4

  numbered_echo "Yielding to couchbase..."

fi

# Attach to couchbase entrypoint
numbered_echo "Attaching to couchbase-server entrypoint"
fg 1