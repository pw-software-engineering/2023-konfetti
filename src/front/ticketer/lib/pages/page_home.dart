import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';
import 'package:ticketer/pages/login/page_login.dart';

import '../auth/account.dart';
import '../model/account_type.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  final String title = "Home Page";

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    Account? user = Auth().getCurrentUser;

    switch (user!.type) {
      case AccountType.Organizer:
        return const OrganizerLandingPage();
      case AccountType.Admin:
      case AccountType.User:
      default:
        return const LoginPage();
    }
  }
}
