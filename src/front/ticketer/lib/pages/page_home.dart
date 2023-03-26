import 'package:flutter/material.dart';
import 'package:ticketer/auth/account.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/model/account_type.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';

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
      case AccountType.User:
      default:
        throw UnimplementedError("This is not implemented yet");
    }
  }
}
