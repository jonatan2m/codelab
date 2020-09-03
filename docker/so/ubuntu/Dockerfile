FROM ubuntu:16.04

LABEL Creator: "DocEvent ssh-keygen"

# update apt-get list, install ssh-keygen and clean packages
RUN apt-get update && apt-get -y install openssh-client \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR keys

RUN ssh-keygen -P "" -f docevent-key

CMD ["bash"]