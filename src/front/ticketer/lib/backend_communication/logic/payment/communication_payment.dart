import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class CommunicationPayment {
  static const String _confirmEndPoint = "/payment/confirm";
  static const String _finishEndPoint = "/payment/finish";

  Future<Tuple2<Response, ResponseCode>> confirm(String paymentId) async {
    return await PaymentCommunication().postCall(
      _confirmEndPoint,
      data: jsonEncode({"id": paymentId}),
    );
  }

  Future<Tuple2<Response, ResponseCode>> finish(String paymentId) async {
    if (Auth().getCurrentAccount == null) {
      throw Exception("Non authorized API call");
    }
    return await BackendCommunication().postCallAuthorized(
      _finishEndPoint,
      Token(Auth().getCurrentAccount!.token),
      data: jsonEncode({"id": paymentId}),
    );
  }
}
