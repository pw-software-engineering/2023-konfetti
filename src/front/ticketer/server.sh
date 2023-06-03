#!/bin/bash

echo BACKEND_URL=$BACKEND_ADDRESS > build/web/assets/assets/dotenv
echo PAYMENT_URL=$PAYMENT_ADDRESS >> build/web/assets/assets/dotenv

cd build/web
python3 -m http.server 4567
