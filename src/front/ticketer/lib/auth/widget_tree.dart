import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/pages/page_login.dart';
import 'package:ticketer/pages/page_home.dart';
import 'package:flutter/material.dart';

class WidgetTree extends StatelessWidget {
  const WidgetTree({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: Auth().authStateChanges,
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          // After logging in
          return const HomePage();
        } else {
          return const LoginPage();
        }
      },
    );
  }
}
