import 'dart:async';

import 'package:ticketer/auth/account.dart';
import 'package:ticketer/auth/auth_provider.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/user.dart';

class Auth {
  static final Auth _singleton = Auth._internal();
  final AuthProvider _provider;

  factory Auth() {
    return _singleton;
  }

  Auth._internal() : _provider = AuthProvider();

  Future<void> init({bool skipSavedToken = false}) async {
    await _provider.init(skipSavedToken: skipSavedToken);
  }

  Future<ResponseCode> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    if (!_provider.isInitialized) throw Exception("Provider not initilized");
    return await _provider.logInWithEmailAndPassword(
        email: email, password: password);
  }

  Future<void> logOut() async {
    if (!_provider.isInitialized) throw Exception("Provider not initilized");
    await _provider.logOut();
  }

  Stream<Account?> get authStateChanges => _provider.authStateChanges;

  Account? get getCurrentAccount => _provider.getCurrentAccount;

  Future<ResponseCode> registerUser(User user) async {
    if (!_provider.isInitialized) throw Exception("Provider not initilized");
    return await _provider.registerUser(user);
  }

  Future<ResponseCode> registerOrganizer(Organizer organizer) async {
    if (!_provider.isInitialized) throw Exception("Provider not initilized");
    return await _provider.registerOrginizer(organizer);
  }
}
