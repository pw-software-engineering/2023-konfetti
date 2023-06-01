import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:tuple/tuple.dart';

class CommunicationPayment {
  static const String _confirmEndPoint = "/payment/confirm";

  Future<Tuple2<Response, ResponseCode>> confirm(String paymentId) async {
    return await PaymentCommunication().postCall(
      _confirmEndPoint,
      data: jsonEncode({"id": paymentId}),
    );
  }
}
