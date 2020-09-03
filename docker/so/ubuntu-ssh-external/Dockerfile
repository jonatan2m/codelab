FROM ubuntu:16.04

LABEL Creator: "DocEvent ssh-keygen"

# update apt-get list, install ssh-keygen and clean packages
RUN apt-get update && apt-get -y install openssh-client \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

CMD ssh-keygen -q -t rsa -N '' -f /keys/id_rsa