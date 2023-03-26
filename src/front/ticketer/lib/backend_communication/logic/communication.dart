// Singleton class for communication
import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/backend_communication/logic/dio_interceptors.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class BackendCommunication {
  static final BackendCommunication _singleton =
      BackendCommunication._internal();
  factory BackendCommunication() {
    return _singleton;
  }
  BackendCommunication._internal();
  bool _isInitialized = false;
  bool get isInitialized => _isInitialized;

  final dio = Dio();
  static const Map<String, dynamic> headers = <String, String>{
    "Access-Control-Allow-Origin": "*",
    'Content-Type': 'application/json',
    'Accept': '*/*'
  };

  // Initialize communication object
  init() async {
    if (_isInitialized) return;
    String? url = dotenv.env['BACKEND_URL'];

    if (url == null) {
      throw Exception("Missing dotenv file");
    }

    // Configure request sending
    dio.options.baseUrl = url;
    dio.options.headers = headers;
    dio.options.receiveDataWhenStatusError = true;
    dio.interceptors.add(CustomInterceptors());
    _isInitialized = true;
  }

  Future<Tuple2<Response, ResponseCode>> postCall(String path,
      {Object? data}) async {
    if (!isInitialized) throw Exception("Not initilized");
    Response response;

    try {
      response = await dio.post(path, data: data);
    } catch (e) {
      log(e.toString());
      throw Exception("Connection error: ${e.toString()}");
    }

    return Tuple2<Response, ResponseCode>(
        response, ResponseCode.getByCode(response.statusCode));
  }
}
