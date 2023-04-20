import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/pages/organizer/organizer_event_list_page.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';

class OrganizerNavigationDrawer extends StatefulWidget {
  const OrganizerNavigationDrawer({
    Key? key,
  }) : super(key: key);

  @override
  State<OrganizerNavigationDrawer> createState() =>
      _OrganizerNavigationDrawerState();
}

class _OrganizerNavigationDrawerState extends State<OrganizerNavigationDrawer> {
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

  // TODO: TM-18 Add real info
  Widget buildHeader(BuildContext context) {
    return Container(
      padding: EdgeInsets.only(
        top: MediaQuery.of(context).padding.top + 24,
        bottom: 18,
      ),
      color: Theme.of(context).primaryColor,
      child: Column(children: const [
        Text(
          "Logged on as organizer",
          style: TextStyle(
            color: Colors.white,
            fontSize: 24,
            fontWeight: FontWeight.bold,
          ),
        )
      ]),
    );
  }

  Future<void> launchCreateEventAction(BuildContext context) async {
    Navigator.push(context,
        MaterialPageRoute(builder: (context) => const OrganizerLandingPage()));
  }

  Future<void> launchMyEventsAction(BuildContext context) async {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => const OrganizerEventListPage()));
  }

  Future<void> launchEditAccount(BuildContext context) async {
    Navigator.pop(context);

    throw UnimplementedError("This is not implemented yet");
  }

  Future<void> signOut(BuildContext context) async {
    Navigator.pop(context);
    await Auth().logOut();
  }

  Widget buildMenuItems(BuildContext context) {
    return Container(
      margin: const EdgeInsets.all(24),
      child: Wrap(
        runSpacing: 16,
        children: [
          ListTile(
            leading: const Icon(Icons.add_box),
            title: const Text("Create Event"),
            onTap: () => {launchCreateEventAction(context)},
          ),
          ListTile(
            leading: const Icon(Icons.event_available_rounded),
            title: const Text("My Events"),
            onTap: () => {launchMyEventsAction(context)},
          ),
          ListTile(
            leading: const Icon(Icons.edit),
            title: const Text("Edit Data"),
            onTap: () => {launchEditAccount(context)},
          ),
          const Divider(
            color: Colors.black54,
          ),
          ListTile(
            leading: const Icon(Icons.logout),
            title: const Text("Sign out"),
            onTap: () => {signOut(context)},
          ),
        ],
      ),
    );
  }
}
