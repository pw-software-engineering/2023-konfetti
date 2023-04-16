import 'dart:convert';

import 'package:dart_jsonwebtoken/dart_jsonwebtoken.dart';
import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http_mock_adapter/http_mock_adapter.dart';
import 'package:ticketer/auth/account.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/logic/dio_interceptors.dart';
import 'package:ticketer/backend_communication/model/account_type.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';

const String loginPath = "/account/login";
const successMessage = {'message': 'Success'};

void main() async {
  final dio = Dio();
  final dioAdapter = DioAdapter(dio: dio, matcher: const UrlRequestMatcher());

  final Credentials correct = Credentials("email@emial.com", "123123Aa");

  late String token;

  late String accId;
  late String role;
  late DateTime nbf;
  late DateTime iat;
  late DateTime exp;

  setUp(() async {
    await dotenv.load(fileName: "assets/dotenv");
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
  group('Auth login tests', () {
    test('Auth should be initilized and login should be possible', () async {
      assert(BackendCommunication().isInitialized == true);

      expect(
          await Auth().logInWithEmailAndPassword(
              email: correct.email, password: correct.password),
          ResponseCode.allGood);

      expect(Auth().getCurrentAccount!.token, token);
      expect(Auth().getCurrentAccount!.accountId, accId);
      expect(Auth().getCurrentAccount!.type, AccountType.User);
    });
  });
}
