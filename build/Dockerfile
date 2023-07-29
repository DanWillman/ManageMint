FROM couchbase:community-7.2.0

COPY config.sh /opt/couchbase
COPY generated.json /opt/couchbase 

RUN apt-get update && apt-get install -y python3

RUN ["chmod", "+x", "/opt/couchbase/config.sh"]
CMD ["/opt/couchbase/config.sh"]