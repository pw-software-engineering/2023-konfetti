import 'dart:convert';

import 'package:dart_jsonwebtoken/dart_jsonwebtoken.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http_mock_adapter/http_mock_adapter.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/logic/dio_interceptors.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';
import 'package:ticketer/pages/organizer/organizer_event_list_page.dart';

Widget _homeWidget() {
  return const MaterialApp(
    home: OrganizerEventListPage(),
  );
}

const String loginPath = "/account/login";

void main() {
  final dio = Dio();
  final dioAdapter = DioAdapter(dio: dio, matcher: const UrlRequestMatcher());

  late String token;

  final Credentials correct = Credentials("email@email.com", "123123Aa");

  late String accId;
  late String role;
  late DateTime nbf;
  late DateTime iat;
  late DateTime exp;

  const Map<String, dynamic> mockedResponse = {
    "items": [
      {
        "id": "string",
        "organizerId": "string",
        "name": "string",
        "description": "string",
        "location": "string",
        "date": "2023-04-16T15:24:09.232Z",
        "sectors": [
          {
            "name": "string",
            "priceInSmallestUnit": 1,
            "numberOfColumns": 1,
            "numberOfRows": 1
          }
        ]
      }
    ],
    "totalCount": 1
  };

  setUp(() async {
    await dotenv.load(fileName: "assets/dotenv");
    FlutterSecureStorage.setMockInitialValues({});
    dio.options.baseUrl = 'http://localhost:8080';
    dio.httpClientAdapter = dioAdapter;
    dio.options.receiveDataWhenStatusError = true;
    dio.interceptors.add(CustomInterceptors());

    accId = "bdc367fd-d4de-4b5d-bec2-d605fd12a891";
    role = "User";
    nbf = DateTime.now().subtract(const Duration(hours: 1));
    iat = DateTime.now().subtract(const Duration(hours: 1));
    exp = DateTime.now().add(const Duration(days: 7));
    token = JWT({
      "AccountId": accId,
      "role": role,
      "nbf": (nbf.millisecondsSinceEpoch / 1000).ceil(),
      "iat": (iat.millisecondsSinceEpoch / 1000).ceil(),
      "exp": (exp.millisecondsSinceEpoch / 1000).ceil(),
    }).sign(SecretKey('secret passphrase')).toString();

    dioAdapter.onGet(
      "/event/organizer/my/list",
      (request) {
        return request.reply(200, mockedResponse);
      },
      data: "",
      headers: BackendCommunication.headers,
      queryParameters: {"PageNumber": 0, "PageSize": 3},
    );

    dioAdapter.onPost(
      loginPath,
      (request) {
        return request.reply(200, {'accessToken': token});
      },
      data: correct.toJson(),
      headers: BackendCommunication.headers,
      queryParameters: {},
    );

    await BackendCommunication().init(altDio: dio);
    await Auth().init(skipSavedToken: true);
  });
  group('Organizer event list page tests', () {
    test('Should return event organizer list of events', () async {
      // given
      // await tester.pumpWidget(_homeWidget());
      assert(BackendCommunication().isInitialized == true);

      await Auth().logInWithEmailAndPassword(
          email: correct.email, password: correct.password);

      var res = await BackendCommunication().event.organizerMyList(0, 3);
      var resJson = res.item1.data;
      expect(resJson["totalCount"], 1);
    });
  });
}
