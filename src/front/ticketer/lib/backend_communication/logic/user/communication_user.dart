import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/user.dart';
import 'package:tuple/tuple.dart';

class UserCommunication {
  static const String _updateEndPoint = "/user/update";
  static const String _registerEndPoint = "/user/register";
  Future<Tuple2<Response, ResponseCode>> update(User body) async =>
      await BackendCommunication()
          .postCall(_updateEndPoint, data: jsonEncode(body));
  Future<Tuple2<Response, ResponseCode>> register(User body) async =>
      await BackendCommunication()
          .postCall(_registerEndPoint, data: jsonEncode(body));
}
