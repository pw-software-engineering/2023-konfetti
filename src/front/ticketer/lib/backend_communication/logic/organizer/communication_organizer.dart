import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class OrganizerCommunication {
  static const String _updateEndPoint = "/organizer/update";
  static const String _registerEndPoint = "/organizer/register";
  Future<Tuple2<Response, ResponseCode>> update(Organizer body) async =>
      await BackendCommunication()
          .postCall(_updateEndPoint, data: jsonEncode(body));
  Future<Tuple2<Response, ResponseCode>> register(Organizer body) async =>
      await BackendCommunication()
          .postCall(_registerEndPoint, data: jsonEncode(body));
}
