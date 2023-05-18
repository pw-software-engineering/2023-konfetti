import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/user.dart';
import 'package:tuple/tuple.dart';

class UserCommunication {
  static const String _updateEndPoint = "/user/update";
  static const String _registerEndPoint = "/user/register";
  Future<Tuple2<Response, ResponseCode>> update(UserUpdate body) async =>
      await BackendCommunication().postCallAuthorized(
          _updateEndPoint, Token(Auth().getCurrentAccount!.token),
          data: jsonEncode(body));
  Future<Tuple2<Response, ResponseCode>> register(User body) async =>
      await BackendCommunication()
          .postCall(_registerEndPoint, data: jsonEncode(body));
}
