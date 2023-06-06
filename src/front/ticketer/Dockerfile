FROM ubuntu:20.04 as build

RUN apt-get update
RUN apt-get install -y git 
# RUN apt-get install -y curl git wget unzip libgconf-2-4 gdb libstdc++6 libglu1-mesa fonts-droid-fallback lib32stdc++6 python3
RUN apt-get clean

# download Flutter SDK from Flutter Github repo
RUN git clone --progress --verbose https://github.com/flutter/flutter.git /usr/local/flutter

RUN apt-get install -y curl unzip wget python3
# Set flutter environment path
ENV PATH="/usr/local/flutter/bin:/usr/local/flutter/bin/cache/dart-sdk/bin:${PATH}"

# Run flutter doctor
RUN flutter doctor

# Enable flutter web
RUN flutter channel master
RUN flutter upgrade
# RUN flutter config --enable-web

# Copy files to container and build
RUN mkdir /app/
COPY . /app/
WORKDIR /app/
RUN flutter pub get
RUN flutter build web



# FROM ubuntu:20.04
# EXPOSE 80

# RUN apt-get update
# RUN apt-get install -y python3

# WORKDIR /app/
# COPY --from=build /app/build/web/ .


# # make server startup script executable and start the web server
# RUN ["chmod", "+x", "/app/server.sh"]

# ENTRYPOINT [ "/app/server.sh"]

FROM nginx
EXPOSE 80

ARG back
ARG payment
ENV BACKEND_ADDRESS $back
ENV PAYMENT_ADDRESS $payment
COPY --from=build /app/build/web/ /usr/share/nginx/html
COPY ./server.sh .
RUN ["chmod", "+x", "./server.sh"]
RUN ["bash", "./server.sh"]

