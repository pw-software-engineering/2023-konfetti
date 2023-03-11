import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/pages/page_login.dart';
import 'package:flutter/material.dart';

class WidgetTree extends StatefulWidget {
  const WidgetTree({Key? key}) : super(key: key);

  @override
  State<WidgetTree> createState() => _WidgetTreeState();
}

class _WidgetTreeState extends State<WidgetTree> {
  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: Auth().authStateChanges,
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          // After logging in
          // return const HomePage();
          throw Exception("How did You get here?");
        } else {
          return const LoginPage();
        }
      },
    );
  }
}
