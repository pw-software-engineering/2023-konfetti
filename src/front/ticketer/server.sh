#!/bin/bash

echo BACKEND_URL=$BACKEND_ADDRESS > assets/assets/dotenv
echo PAYMENT_URL=$PAYMENT_ADDRESS >> assets/assets/dotenv

python3 -m http.server 4567
