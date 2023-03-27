
import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';

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
            leading: const Icon(Icons.logout),
            title: const Text("Sign out"),
            onTap: () => {signOut(context)},
          ),
        ],
      ),
    );
  }
}