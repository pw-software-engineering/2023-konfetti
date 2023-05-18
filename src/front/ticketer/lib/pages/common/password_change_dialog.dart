import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';

Future<void> changePasswordDialog(BuildContext context) async {
  await showDialog(
    context: context,
    builder: (context) {
      return AlertDialog(
        title: const Text("Change password"),
        content: PasswordEntry(),
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

class PasswordEntry extends StatefulWidget {
  @override
  State<PasswordEntry> createState() => _PasswordEntryState();
}

class _PasswordEntryState extends State<PasswordEntry> {
  bool _passwordVisible = false;

  final TextEditingController _controllerPassword = TextEditingController();
  final TextEditingController _controllerPasswordRepeat =
      TextEditingController();

  final _formKey = GlobalKey<FormState>();

  Widget _passwordCreateEntryFiled() {
    return TextFormField(
      obscureText: !_passwordVisible,
      controller: _controllerPassword,
      decoration: InputDecoration(
        labelText: 'Password',
        hintText: 'Enter your password',
        suffixIcon: IconButton(
          icon: Icon(
            _passwordVisible ? Icons.visibility : Icons.visibility_off,
            color: Theme.of(context).primaryColorDark,
          ),
          onPressed: () {
            setState(() {
              _passwordVisible = !_passwordVisible;
            });
          },
        ),
      ),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter password";
        } else if (value.length < 8 || value.length > 32) {
          return "Password length needs to be betweeen 8 and 32 characters";
        } else if (!isPasswordValid(value)) {
          return "Password must contain digit, "
              "small and capital letter";
        }
        return null;
      },
    );
  }

  bool isPasswordValid(String password) {
    return password.contains(RegExp('[0-9]')) &&
        password.contains(RegExp('[a-z]')) &&
        password.contains(RegExp('[A-Z]'));
  }

  Widget _passwordCreateRepeatEntryFiled() {
    return TextFormField(
      obscureText: !_passwordVisible,
      controller: _controllerPasswordRepeat,
      decoration: const InputDecoration(
        labelText: "Repeat password",
        hintText: 'Repeat your password',
      ),
      validator: (value) {
        if (value != _controllerPassword.text) {
          return "Passwords do not match";
        }
        return null;
      },
    );
  }

  Future<void> uploadNewPassword() async {
    if (_formKey.currentState!.validate()) {
      String password = _controllerPassword.text;
      var response = await Auth().changePassword(password);
      if (response.value != 200) {
        await showDilogAfterUnsuccesfullChange(
          response.getResponseString(),
        );
      } else {
        await showDilogAfterChange();
      }
      if (!mounted) return;
      Navigator.pop(context);
    }
  }

  Widget _submitButton() {
    return Container(
      margin: const EdgeInsets.only(top: 15.0),
      child: ElevatedButton(
        onPressed: uploadNewPassword,
        child: const Text("Change password"),
      ),
    );
  }

  Widget _getContent() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          _passwordCreateEntryFiled(),
          _passwordCreateRepeatEntryFiled(),
          _submitButton(),
        ],
      ),
    );
  }

  Future<void> showDilogAfterChange() async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Thank you"),
          content: const Text("You have changed your password"),
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

  Future<void> showDilogAfterUnsuccesfullChange(String errorMess) async {
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
    return Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        const Text("Please enter new password"),
        _getContent(),
      ],
    );
  }
}
