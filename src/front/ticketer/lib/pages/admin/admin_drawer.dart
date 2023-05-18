
import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/pages/admin/events/admin_verify_events_page.dart';
import 'package:ticketer/pages/admin/landing/admin_landing_page.dart';

class AdminNavigationDrawer extends StatefulWidget {
  const AdminNavigationDrawer({
        Key? key,
      }) : super(key: key);

  @override
  State<AdminNavigationDrawer> createState() => _AdminNavigationDrawerState();
}

class _AdminNavigationDrawerState extends State<AdminNavigationDrawer> {
  late void Function() setDataNull;
  late void Function(String) setCvDataName;
  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            buildHeader(context),
            buildMenuItems(context),
          ],
        ),
      ),
    );
  }

  Widget buildHeader(BuildContext context) {
    return Container(
      padding: EdgeInsets.only(
        top: MediaQuery.of(context).padding.top + 24,
        bottom: 18,
      ),
      color: Theme.of(context).primaryColor,
      child: Column(
        children:
             const [
          Text(
            "Logged on as admin",
            style: TextStyle(
              color: Colors.white,
              fontSize: 24,
              fontWeight: FontWeight.bold,
            ),
          )
        ]
      ),
    );
  }

  Future<void> launchEditAccount(BuildContext context) async {
    Navigator.pop(context);

    throw UnimplementedError("This is not implemented yet");
  }

  Future<void> signOut(BuildContext context) async {
    Navigator.of(context).popUntil((route) => route.isFirst);
    await Auth().logOut();
  }

  Future<void> launchVerifyOrganizers(BuildContext context) async {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => const AdminLandingPage()));
  }

  Future<void> launchVerifyEvents(BuildContext context) async {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => const AdminVerifyEventsPage()));
  }

  Widget buildMenuItems(BuildContext context) {
    return Container(
      margin: const EdgeInsets.all(24),
      child: Wrap(
        runSpacing: 16,
        children: [
          ListTile(
            leading: const Icon(Icons.logout),
            title: const Text("Sign out"),
            onTap: () => {signOut(context)},
          ),
          const Divider(
            color: Colors.black54,
          ),
          ListTile(
            leading: const Icon(Icons.account_box),
            title: const Text("Organizers to verify"),
            onTap: () => {launchVerifyOrganizers(context)},
          ),
          ListTile(
            leading: const Icon(Icons.calendar_month),
            title: const Text("Events to verify"),
            onTap: () => {launchVerifyEvents(context)},
          ),
        ],
      ),
    );
  }
}