import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class EventCommunication {
  static const String _createEndPoint = "/event/create";
  static const String _listEndPoint = "/event/list";
  static const String _organizerListEndPoint = "/event/organizer/my/list";

  Future<Tuple2<Response, ResponseCode>> create(Event body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _createEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode(body));
  }

  Future<Tuple2<Response, ResponseCode>> list(int pageNo, int pageSize) async {
    Map<String, dynamic> params = {"PageNumber": pageNo, "PageSize": pageSize};
    return await BackendCommunication().getCallAuthorized(
        _listEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }

  Future<Tuple2<Response, ResponseCode>> organizerMyList(
      int pageNo, int pageSize) async {
    Map<String, dynamic> params = {"PageNumber": pageNo, "PageSize": pageSize};
    return await BackendCommunication().getCallAuthorized(
        _organizerListEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }
}
