import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/sector.dart';
import 'package:tuple/tuple.dart';

import '../communication.dart';

class TicketCommunication {
  static const String _buyEndPoint = "/ticket/buy";
  static const String _listEndPoint = "/ticket/list";

  Future<Tuple2<Response, ResponseCode>> buy(
      Event event, Sector sector, int count) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _buyEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode({
          "eventId": event.id,
          "sectorName": sector.name,
          "numberOfSeats": count
        }));
  }

  Future<Tuple2<Response, ResponseCode>> list(
      int pageNumber, int pageSize) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().getCallAuthorized(
        _listEndPoint, Token(Auth().getCurrentAccount!.token),
        params: {"PageNumber": pageNumber, "PageSize": pageSize});
  }
}
