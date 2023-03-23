import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/user.dart';
import 'package:ticketer/model/user_type.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';
import 'package:ticketer/pages/page_home.dart';
import 'package:flutter/material.dart';

import '../pages/login/page_login.dart';

class WidgetTree extends StatelessWidget {
  const WidgetTree({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return StreamBuilder(
      stream: Auth().authStateChanges,
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          return const HomePage();
        } else {
          return const LoginPage();
        }
      },
    );
  }
}
