import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class OrganizerCommunication {

  static const String _updateEndPoint = "/organizer/update";
  static const String _registerEndPoint = "/organizer/register";
  static const String _listEndPoint = "/organizer/list";

  Future<Tuple2<Response, ResponseCode>> update(Organizer body) async =>
      await BackendCommunication()
          .postCall(_updateEndPoint, data: jsonEncode(body));

  Future<Tuple2<Response, ResponseCode>> register(Organizer body) async =>
      await BackendCommunication()
          .postCall(_registerEndPoint, data: jsonEncode(body));

  Future<Tuple2<Response, ResponseCode>> listToVerify(int pageNumber, int pageSize) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    String query = "$_listEndPoint?PageNumber=$pageNumber&PageSize=$pageSize&ShowAscending=true&SortBy=0&VerificationStatusesFilter=0";
    return await BackendCommunication().getCallAuthorized(
        query, Token(Auth().getCurrentAccount!.token));
  }
}
