import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/organizer_account.dart';
import 'package:ticketer/backend_communication/model/organizer_verification_status.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

import '../../model/organizer.dart';

class OrganizerCommunication {
  static const String _updateEndPoint = "/organizer/update";
  static const String _registerEndPoint = "/organizer/register";
  static const String _listEndPoint = "/organizer/list";
  static const String _decideEndPoint = "/organizer/decide";
  static const String _viewEndPoint = "/organizer/view";

  Future<Tuple2<Response, ResponseCode>> update(
      OrganizerAccountUpdate body) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _updateEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode(body));
  }

  Future<Tuple2<Response, ResponseCode>> register(
          OrganizerAccount body) async =>
      await BackendCommunication()
          .postCall(_registerEndPoint, data: jsonEncode(body));

  Future<Tuple2<Response, ResponseCode>> list(
      int pageNumber, int pageSize) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    Map<String, dynamic> params = {
      "PageNumber": pageNumber,
      "PageSize": pageSize,
      "ShowAscending": true,
      "SortBy": 0,
      "VerificationStatusesFilter": OrganizerVerificationStatus.Waiting.index
    };
    return await BackendCommunication().getCallAuthorized(
        _listEndPoint, Token(Auth().getCurrentAccount!.token),
        params: params);
  }

  Future<Tuple2<Response, ResponseCode>> decide(
      Organizer organizer, bool isAccepted) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
        _decideEndPoint, Token(Auth().getCurrentAccount!.token),
        data: jsonEncode(
            {"organizerId": organizer.id, "isAccepted": isAccepted}));
  }

  Future<Tuple2<Response, ResponseCode>> view() async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().getCallAuthorized(
        _viewEndPoint, Token(Auth().getCurrentAccount!.token));
  }
}
