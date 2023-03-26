import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';
import 'package:ticketer/backend_communication/model/user.dart';

class UserRegisterPage extends StatefulWidget {
  Credentials credentials;

  UserRegisterPage({Key? key, required this.credentials}) : super(key: key);

  @override
  State<UserRegisterPage> createState() => _UserDataState();
}

class _UserDataState extends State<UserRegisterPage> {
  final TextEditingController _firstName = TextEditingController();
  final TextEditingController _lastName = TextEditingController();
  final TextEditingController _birthDate = TextEditingController();
  late Credentials credentials;

  final _formKey = GlobalKey<FormState>();

  @override
  void initState() {
    credentials = widget.credentials;
    super.initState();
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
          onPressed: () => submitUserData(),
          child: const Text("Submit"),
        ));
  }

  Widget _getRegisterContent() {
    return Form(
      key: _formKey,
      child: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            _titleBanner(),
            _firstNameEntryField(),
            _lastNameEntryField(),
            _birthDateEntryField(),
            _submitButton(),
          ],
        ),
      ),
    );
  }

  Text _titleBanner() {
    return const Text(
      "Join us as a user",
      style: TextStyle(
        fontSize: 22,
        fontWeight: FontWeight.w600,
        color: Colors.blueAccent,
      ),
    );
  }

  Future<void> submitUserData() async {
    if (_formKey.currentState!.validate()) {
      User user = User(_firstName.text, _lastName.text, _birthDate.text,
          credentials.email, credentials.password);

      var response = await Auth().registerUser(user);
      if (response.value != 200) {
        await showDilogAfterUnsuccesfullRegistration(
            response.getResponseString());
      } else {
        await showDilogAfterRegistration();
      }
      if (!mounted) return;
      Navigator.of(context).popUntil((route) => route.isFirst);
    }
  }

  Future<void> showDilogAfterRegistration() async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Thank you"),
          content: const Text("You can sign in into the account now"),
          actions: [
            ElevatedButton(
              onPressed: () => {
                Navigator.pop(context),
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  Future<void> showDilogAfterUnsuccesfullRegistration(String errorMess) async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Something went wrong"),
          content: Text("Your request faced an error: $errorMess"),
          actions: [
            ElevatedButton(
              onPressed: () => {
                Navigator.pop(context),
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Register as a user"),
      ),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 400),
          padding: const EdgeInsets.all(20),
          child: _getRegisterContent(),
        ),
      ),
    );
  }
}

Future<DateTime?> _selectDate(BuildContext context, DateTime initial) async {
  return await showDatePicker(
      context: context,
      initialDate: initial,
      firstDate: DateTime(2015, 8),
      lastDate: DateTime(2101));
}

class DataDialog extends StatefulWidget {
  @override
  State<DataDialog> createState() => _DataDialogState();

  final String field;

  const DataDialog(this.field, {super.key});
}

class _DataDialogState extends State<DataDialog> {
  String field = "";
  String content = "";

  @override
  void initState() {
    super.initState();
    field = widget.field;
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text(field),
      content: TextField(
        maxLines: null,
        onChanged: (value) => setState(() {
          content = value;
        }),
      ),
      actions: [
        ElevatedButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cancel')),
        ElevatedButton(
            onPressed: () => Navigator.pop(context, content),
            child: const Text('Confirm')),
      ],
    );
  }
}
