import 'package:flutter/material.dart';
import 'package:ticketer/auth/account.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/model/account_type.dart';
import 'package:ticketer/pages/admin/admin_landing_page.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';
import 'package:ticketer/pages/user/user_landing_page.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  final String title = "Home Page";

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    Account? user = Auth().getCurrentAccount;

    switch (user!.type) {
      case AccountType.Organizer:
        return const OrganizerLandingPage();
      case AccountType.Admin:
        return const AdminLandingPage();
      case AccountType.User:
        return const UserLandingPage();
      default:
        throw UnimplementedError("This is not implemented yet");
    }
  }
}
