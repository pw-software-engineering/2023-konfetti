import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/user.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/common/event_tile.dart';
import 'package:ticketer/pages/user/user_drawer.dart';

class UserDataEdit extends StatefulWidget {
  const UserDataEdit({Key? key}) : super(key: key);

  @override
  State<UserDataEdit> createState() => _UserDataEditState();
}

class _UserDataEditState extends State<UserDataEdit> {
  final TextEditingController _firstName = TextEditingController();
  final TextEditingController _lastName = TextEditingController();
  final TextEditingController _birthDate = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getUserIcon(),
          _getGreeting(),
          _getEditContent(),
        ],
      ),
    );
  }

  Text _getGreeting() {
    return Text(
      "Hello user, edit data",
      style: TextStyle(
        fontSize: 26,
        color: Theme.of(context).primaryColor,
      ),
    );
  }

  Container _getUserIcon() {
    return Container(
      height: 60,
      width: 60,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        color: Theme.of(context).primaryColor,
      ),
      alignment: Alignment.center,
      child: const Icon(
        Icons.person,
        color: Colors.white,
        size: 40,
      ),
    );
  }

  Widget _getEditContent() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          _firstNameEntryField(),
          _lastNameEntryField(),
          _birthDateEntryField(),
          _submitButton(),
        ],
      ),
    );
  }

  Widget _firstNameEntryField() {
    return TextFormField(
      controller: _firstName,
      decoration: const InputDecoration(
          labelText: "First Name", hintText: 'Enter your first name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter your first name";
        }
        return null;
      },
    );
  }

  Widget _lastNameEntryField() {
    return TextFormField(
      controller: _lastName,
      decoration: const InputDecoration(
          labelText: "Last Name", hintText: 'Enter your last name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter your last name";
        }
        return null;
      },
    );
  }

  Widget _birthDateEntryField() {
    return TextFormField(
      controller: _birthDate,
      decoration: const InputDecoration(
          labelText: "Birth Date", hintText: 'Choose your birth date'),
      readOnly: true,
      onTap: () => _selectDate(context, DateTime.now()).then((date) => {
            if (date != null)
              _birthDate.text = '${date.year}-${date.month}-${date.day}'
          }),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please choose your birth date";
        }
        return null;
      },
    );
  }

  Widget _submitButton() {
    return Container(
        margin: const EdgeInsets.only(top: 15.0),
        child: ElevatedButton(
          onPressed: () => {},
          child: const Text("Submit"),
        ));
  }

  Future<DateTime?> _selectDate(BuildContext context, DateTime initial) async {
    const daysInAYear = 365;
    const maxAge = 100;
    return await showDatePicker(
        context: context,
        initialDate: initial,
        firstDate:
            DateTime.now().subtract(const Duration(days: maxAge * daysInAYear)),
        lastDate: DateTime.now());
  }

  Future<void> submitUserData() async {
    if (_formKey.currentState!.validate()) {
      User user =
          User(_firstName.text, _lastName.text, _birthDate.text, "", "");
      if (!mounted) return;
      Navigator.of(context).popUntil((route) => route.isFirst);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Edit data"),
      ),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 400),
          child: _getContent(),
        ),
      ),
    );
  }
}
