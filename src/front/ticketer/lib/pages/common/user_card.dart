import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/model/user.dart';

class UserCard extends StatefulWidget {
  final User user;

  const UserCard({Key? key, required this.user}) : super(key: key);

  @override
  State<UserCard> createState() => _UserCardState();
}

class _UserCardState extends State<UserCard> {
  late User user;

  @override
  void initState() {
    user = widget.user;
    super.initState();
  }

  Container _userContainer() {
    return Container(
      padding: const EdgeInsets.all(9.0),
      decoration: BoxDecoration(
          border: Border.all(
            color: Theme.of(context).hintColor,
          )),
      child: SingleChildScrollView(
        child: Column(
          children: [
            _getUserInfo("First name", user.firstName),
            _getUserInfo("Last name", user.lastName),
            _getUserInfo("Email", user.email),
            _getUserInfo("Birth Date", user.birthDate)
          ],
        ),
      ),
    );
  }

  Widget _getUserInfo(String property, String value) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Flexible(
          flex: 2,
          child: Text(
            property,
            style: TextStyle(
                fontWeight: FontWeight.w700,
                color: Theme.of(context).primaryColor,
                fontSize: 16),
          ),
        ),
        Flexible(
          flex: 5,
          child: Text(
            value,
            style: const TextStyle(fontSize: 16),
          ),
        ),
      ],
    );
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [_userContainer()],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
      padding: const EdgeInsets.all(20),
      child: _getContent(),
    );
  }
}
