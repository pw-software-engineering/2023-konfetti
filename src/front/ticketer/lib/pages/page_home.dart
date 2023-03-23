import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/auth/user.dart';
import 'package:ticketer/model/user_type.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';
import 'package:ticketer/pages/page_login.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  final String title = "Home Page";

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    User? user = Auth().getCurrentUser;

    switch (user!.type) {
      case UserType.Organizer:
        return const OrganizerLandingPage();
      case UserType.Admin:
      case UserType.User:
      default:
        return const LoginPage();
    }
  }
}
