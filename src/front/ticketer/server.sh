#!/bin/bash

echo BACKEND_URL=$BACKEND_ADDRESS > /usr/share/nginx/html/assets/assets/dotenv
echo PAYMENT_URL=$PAYMENT_ADDRESS >> /usr/share/nginx/html/assets/assets/dotenv
