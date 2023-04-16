import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class AccountCommunication {
  static const String _loginEndPoint = "/account/login";
  Future<Tuple2<Response, ResponseCode>> login(Credentials body) async =>
      await BackendCommunication()
          .postCall(_loginEndPoint, data: body.toJson());
}
