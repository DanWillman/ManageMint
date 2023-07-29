FROM couchbase:community-7.2.0

COPY couchbase-config-new.sh /opt/couchbase
COPY generated.json /opt/couchbase

RUN apt-get update && apt-get install -y python3

RUN ["chmod", "+x", "/opt/couchbase/couchbase-config-new.sh"]
CMD ["/opt/couchbase/couchbase-config-new.sh"]