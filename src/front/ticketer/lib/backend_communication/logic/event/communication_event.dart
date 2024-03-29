import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/event_status.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class EventCommunication {
  static const String _createEndPoint = "/event/create";
  static const String _listEndPoint = "/event/list";
  static const String _organizerListEndPoint = "/event/organizer/my/list";
  static const String _editEndPoint = "/event/update";
  static const String _decideEndPoint = "/event/decide";
  static const String _publishEndPoint = "/event/publish";
  static const String _saleStartEndPoint = "/event/sale/start";
  static const String _saleStopEndPoint = "/event/sale/stop";

  Future<Tuple2<Response, ResponseCode>> create(Event body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _createEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode(body));
  }

  Future<Tuple2<Response, ResponseCode>> list(int pageNo, int pageSize) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    Map<String, dynamic> params = {"PageNumber": pageNo, "PageSize": pageSize};
    return await BackendCommunication().getCallAuthorized(
        _listEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }

  Future<Tuple2<Response, ResponseCode>> organizerMyList(
      int pageNo, int pageSize) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    Map<String, dynamic> params = {"PageNumber": pageNo, "PageSize": pageSize};
    return await BackendCommunication().getCallAuthorized(
        _organizerListEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }

  Future<Tuple2<Response, ResponseCode>> edit(Event body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _editEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode(body));
  }

  Future<Tuple2<Response, ResponseCode>> listToVerify(
      int pageNumber, int pageSize) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    Map<String, dynamic> params = {
      "PageNumber": pageNumber,
      "PageSize": pageSize,
      "ShowAscending": true,
      "SortBy": 0,
      "EventStatusesFilter": EventStatus.Unverified.index
    };
    return await BackendCommunication().getCallAuthorized(
        _listEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }

  Future<Tuple2<Response, ResponseCode>> listFiltered(
      int pageNumber,
      int pageSize,
      String name,
      String location,
      String earlier,
      String later,
      EventStatus status) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    Map<String, dynamic> params = {
      "PageNumber": pageNumber,
      "PageSize": pageSize,
      "ShowAscending": true,
      "SortBy": 0,
      "EventNameFilter": name,
      "Location": location,
      "EventStatusesFilter": status.index
    };
    if (earlier.isNotEmpty) {
      params.addAll({"EarlierThanInclusiveFilter": earlier});
    }
    if (later.isNotEmpty) {
      params.addAll({"LaterThanInclusiveFilter": later});
    }
    return await BackendCommunication().getCallAuthorized(
        _listEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }

  Future<Tuple2<Response, ResponseCode>> decide(
      Event event, bool isAccepted) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _decideEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode({"id": event.id, "isAccepted": isAccepted}));
  }

  Future<Tuple2<Response, ResponseCode>> publish(Event body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _publishEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode({"id": body.id}));
  }

  Future<Tuple2<Response, ResponseCode>> saleStart(Event body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _saleStartEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode({"eventId": body.id}));
  }

  Future<Tuple2<Response, ResponseCode>> saleStop(Event body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _saleStopEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode({"eventId": body.id}));
  }
}
