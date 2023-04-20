// Singleton class for communication
import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/account/communication_account.dart';
import 'package:ticketer/backend_communication/logic/dio_interceptors.dart';
import 'package:ticketer/backend_communication/logic/event/communication_event.dart';
import 'package:ticketer/backend_communication/logic/organizer/communication_organizer.dart';
import 'package:ticketer/backend_communication/logic/user/communication_user.dart';
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

  final UserCommunication _userCommunication = UserCommunication();
  UserCommunication get user => _userCommunication;
  final OrganizerCommunication _organizerCommunication =
      OrganizerCommunication();
  OrganizerCommunication get organizer => _organizerCommunication;
  final AccountCommunication _accountCommunication = AccountCommunication();
  AccountCommunication get account => _accountCommunication;
  final EventCommunication _eventCommunication = EventCommunication();
  EventCommunication get event => _eventCommunication;

  late final Dio dio;
  static Map<String, dynamic> headers = <String, String>{
    "Access-Control-Allow-Origin": "*",
    'Content-Type': 'application/json',
    'Accept': '*/*'
  };

  // Initialize communication object
  init({Dio? altDio}) async {
    if (_isInitialized) return;
    String? url = dotenv.env['BACKEND_URL'];

    if (url == null) {
      throw Exception("Missing dotenv file");
    }

    // Configure request sending
    if (altDio == null) {
      dio = Dio();
      dio.options.baseUrl = url;
      dio.options.headers = headers;
      dio.options.receiveDataWhenStatusError = true;
      dio.interceptors.add(CustomInterceptors());
    } else {
      dio = altDio;
    }
    _isInitialized = true;
  }

  Future<Tuple2<Response, ResponseCode>> getCallAuthorized(
      String path, Token token,
      {Map<String, dynamic>? params}) async {
    if (!isInitialized) throw Exception("Not initilized");
    Response response;
    var options = dio.options;

    try {
      if (!token.isValid) {
        throw ArgumentError("Token is not valid");
      }
      dio.options.headers.addAll({"Authorization": "Bearer ${token.token}"});
      dio.options.contentType = Headers.formUrlEncodedContentType;
      response = await dio.get(path, queryParameters: params);
    } on DioError catch (e) {
      log(e.toString());
      if (!token.isValid) {
        response = Response(requestOptions: RequestOptions(), statusCode: 401);
      } else if (e.response != null) {
        response = e.response!;
      } else {
        response = Response(requestOptions: RequestOptions());
      }
    } finally {
      dio.options = options;
    }

    return Tuple2<Response, ResponseCode>(
        response, ResponseCode.getByCode(response.statusCode ?? -1));
  }

  Future<Tuple2<Response, ResponseCode>> postCall(String path,
      {Object? data}) async {
    if (!isInitialized) throw Exception("Not initilized");
    Response response;

    try {
      response = await dio.post(path, data: data);
    } on DioError catch (e) {
      log(e.toString());
      if (e.response != null) {
        response = e.response!;
      } else {
        response = Response(requestOptions: RequestOptions());
      }
    }

    return Tuple2<Response, ResponseCode>(
        response, ResponseCode.getByCode(response.statusCode ?? -1));
  }

  Future<Tuple2<Response, ResponseCode>> postCallAuthorized(
      String path, Token token,
      {Object? data}) async {
    if (!isInitialized) throw Exception("Not initilized");
    Response response;
    var options = dio.options;

    try {
      if (!token.isValid) {
        throw ArgumentError("Token is not valid");
      }
      dio.options.headers.addAll({"Authorization": "Bearer ${token.token}"});
      dio.options.contentType = Headers.jsonContentType;
      response = await dio.post(path, data: data);
    } on DioError catch (e) {
      log(e.toString());
      if (!token.isValid) {
        response = Response(requestOptions: RequestOptions(), statusCode: 401);
      } else if (e.response != null) {
        response = e.response!;
      } else {
        response = Response(requestOptions: RequestOptions());
      }
    } finally {
      dio.options = options;
    }

    return Tuple2<Response, ResponseCode>(
        response, ResponseCode.getByCode(response.statusCode ?? -1));
  }
}
