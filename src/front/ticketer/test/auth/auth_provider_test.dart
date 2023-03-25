import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:http_mock_adapter/http_mock_adapter.dart';
import 'package:mockito/mockito.dart';
import 'package:ticketer/auth/account.dart';
import 'package:ticketer/auth/auth_provider.dart';
import 'package:ticketer/model/account_type.dart';

class MockAuthProvider extends Mock implements AuthProvider {}

void main() async {
  late Dio dio;
  late DioAdapter dioAdapter;

  await dotenv.load(fileName: "dotenv");

  group('Accounts', () {
    String? baseUrl = dotenv.env['BACKEND_URL'] ?? "http://localhost:5166";

    const tokenReturn = <String, dynamic>{
      'accessToken':
          'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiJiZGMzNjdmZC1kNGRlLTRiNWQtYmVjMi1kNjA1ZmQxMmE4OTEiLCJyb2xlIjoiVXNlciIsIm5iZiI6MTY3OTUyNjEwNiwiZXhwIjoxNjc5NTQwNTA2LCJpYXQiOjE2Nzk1MjYxMDZ9.PXgKAaju2fEZ-ZRkiic5FfwfjpmXL03OhBciRibn1J0',
    };
    const userCredentials = <String, dynamic>{
      'email': 'test@example.com',
      'password': '123123Aa',
    };
    var returnAccount = Account(AccountType.User,
        "bdc367fd-d4de-4b5d-bec2-d605fd12a891", tokenReturn["accessToken"]);

    setUp(() {
      dio = Dio(BaseOptions(baseUrl: baseUrl));
      dioAdapter = DioAdapter(dio: dio);
    });

    test('Log in', () async {
      const route = '/account/login';

      dioAdapter.onPost(
        route,
        (server) => server.reply(200, tokenReturn),
        data: userCredentials,
      );

      MockAuthProvider provider = MockAuthProvider();

      when(provider.initialized).thenReturn(true);
      when(provider.dio).thenReturn(dio);
      when(provider.isTokenValid(any)).thenReturn(true);

      expectLater(provider.authStateChanges, emits(returnAccount));

      await provider.logInWithEmailAndPassword(
          email: 'test@example.com', password: '123123Aa');

      assert(provider.getCurrentAccount!.accountId == returnAccount.accountId);
      assert(provider.getCurrentAccount!.type == returnAccount.type);
      assert(provider.getCurrentAccount!.token == returnAccount.token);
    });

    // TODO:
    // - Add test for registration of user
    // - Add test for registration of organizer
  });
}
