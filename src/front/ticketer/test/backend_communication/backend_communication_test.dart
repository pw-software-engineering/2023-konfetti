import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:http_mock_adapter/http_mock_adapter.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/logic/dio_interceptors.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';
import 'package:ticketer/backend_communication/model/user.dart';

const organizerRegisterPath = "/organizer/register";
const userRegisterPath = "/user/register";
const successMessage = {'message': 'Success'};

void main() async {
  final dio = Dio();
  final dioAdapter = DioAdapter(dio: dio, matcher: const UrlRequestMatcher());

  var baseUrl;

  setUp(() async {
    await dotenv.load(fileName: "assets/dotenv");
    baseUrl = 'http://localhost:8080';
    dio.options.baseUrl = baseUrl;
    dio.httpClientAdapter = dioAdapter;
    //dio.options.headers = BackendCommunication.headers;
    dio.options.receiveDataWhenStatusError = true;
    dio.interceptors.add(CustomInterceptors());
  });
  group('BackendCommunication tests', () {
    test('Without initialization BackendCommunication() should throw exception',
        () {
      assert(BackendCommunication().isInitialized == false);
      var organizer = Organizer(
          "companyName",
          "address",
          TaxType.NIP,
          "123123123",
          "displayName",
          "email@email.com",
          "password",
          "123123123");
      var user = User(
          "firstName", "lastName", "2020-10-10", "email@email.com", "password");
      expect(() => BackendCommunication().postCall("account/login"),
          throwsA(anything));
      expect(() => BackendCommunication().organizer.register(organizer),
          throwsA(anything));
      expect(() => BackendCommunication().organizer.update(organizer),
          throwsA(anything));
      expect(
          () => BackendCommunication().user.register(user), throwsA(anything));
      expect(() => BackendCommunication().user.update(user), throwsA(anything));
      expect(
          () => BackendCommunication()
              .account
              .login(Credentials("email@email.com", "password")),
          throwsA(anything));
    });
    test('Test register organizer', () async {
      var organizer = Organizer(
          "companyName",
          "address",
          TaxType.NIP,
          "123123123",
          "displayName",
          "email@email.com",
          "password",
          "123123123");
      dioAdapter.onPost(
        organizerRegisterPath,
        (request) {
          return request.reply(200, successMessage);
        },
        data: organizer.toJson(),
        queryParameters: {},
        headers: BackendCommunication.headers,
      );
      if (!BackendCommunication().isInitialized) {
        await BackendCommunication().init(altDio: dio);
      }
      assert(BackendCommunication().isInitialized == true);
      var respo = await BackendCommunication().organizer.register(organizer);

      expect(respo.item2, ResponseCode.allGood);
    });
    test('Test register user', () async {
      var user = User(
          "firstName", "lastName", "2020-10-10", "email@email.com", "password");
      dioAdapter.onPost(
        userRegisterPath,
        (request) {
          return request.reply(200, successMessage);
        },
        data: user.toJson(),
        queryParameters: {},
        headers: BackendCommunication.headers,
      );
      if (!BackendCommunication().isInitialized) {
        await BackendCommunication().init(altDio: dio);
      }
      assert(BackendCommunication().isInitialized == true);
      var respo = await BackendCommunication().user.register(user);

      expect(respo.item2, ResponseCode.allGood);
    });
  });
}
