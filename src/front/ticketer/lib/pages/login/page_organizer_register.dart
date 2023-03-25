import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/model/credentials.dart';
import 'package:ticketer/model/organizer.dart';
import 'package:ticketer/model/tax_type.dart';
import 'dart:convert';
import 'package:http/http.dart';
import 'package:ticketer/pages/login/page_login.dart';

class OrganizerRegisterPage extends StatefulWidget {
  Credentials credentials;

  OrganizerRegisterPage({Key? key, required this.credentials})
      : super(key: key);

  @override
  State<OrganizerRegisterPage> createState() => _OrganizerDataState();
}

class _OrganizerDataState extends State<OrganizerRegisterPage> {
  final TextEditingController _companyName = TextEditingController();
  final TextEditingController _addressCity = TextEditingController();
  final TextEditingController _addressZip = TextEditingController();
  final TextEditingController _addressStreet = TextEditingController();
  final TextEditingController _taxId = TextEditingController();
  final TextEditingController _displayName = TextEditingController();
  final TextEditingController _phone = TextEditingController();
  late Credentials credentials;

  final _formKey = GlobalKey<FormState>();

  late TaxType taxType = TaxType.NIP;
  void setTaxType(TaxType t) {
    taxType = t;
  }

  @override
  void initState() {
    credentials = widget.credentials;
    super.initState();
  }

  Widget _companyNameEntryField() {
    return TextFormField(
      controller: _companyName,
      decoration: const InputDecoration(
          labelText: "Company name", hintText: 'Enter your company name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter company name";
        }
        return null;
      },
    );
  }

  Widget _companyCityEntryField() {
    return TextFormField(
      controller: _addressCity,
      decoration:
          const InputDecoration(labelText: "City", hintText: 'Enter city'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter city";
        }
        return null;
      },
    );
  }

  Widget _companyZipCodeEntryField() {
    return TextFormField(
      controller: _addressZip,
      decoration: const InputDecoration(
          labelText: "Postal / Zip code", hintText: 'Enter postal / zip code'),
      validator: (value) {
        if (value == null ||
            value.isEmpty ||
            !RegExp(r"^\d{2}-\d{3}$", caseSensitive: false).hasMatch(value)) {
          return "Please enter correct code in XX-XXX format";
        }
        return null;
      },
    );
  }

  Widget _companyStreetEntryField() {
    return TextFormField(
      controller: _addressStreet,
      decoration: const InputDecoration(
          labelText: "Address", hintText: 'Enter street and home number'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter address";
        }
        return null;
      },
    );
  }

  Widget _companyDisplayNameEntryField() {
    return TextFormField(
      controller: _displayName,
      decoration: const InputDecoration(
          labelText: "Display name", hintText: 'Enter displayed name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter name";
        }
        return null;
      },
    );
  }

  Widget _companyTaxIdEntryField() {
    return TextFormField(
      controller: _taxId,
      decoration: const InputDecoration(
          labelText: "Tax id number",
          hintText: 'Enter tax identification number'),
      validator: (value) {
        if (value == null ||
            value.isEmpty ||
            !RegExp(r"^\d*$", caseSensitive: false).hasMatch(value)) {
          return "Please enter tax identification number";
        }
        return null;
      },
    );
  }

  Widget _companyPhoneEntryField() {
    return TextFormField(
      controller: _phone,
      decoration: const InputDecoration(
          labelText: "Phone", hintText: 'Enter phone number'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter phone number";
        }
        return null;
      },
    );
  }

  Widget _submitButton() {
    return Container(
        margin: const EdgeInsets.only(top: 15.0),
        child: ElevatedButton(
          onPressed: () => submitOrganizerData(),
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
            _companyNameEntryField(),
            _companyCityEntryField(),
            _companyZipCodeEntryField(),
            _companyStreetEntryField(),
            _companyDisplayNameEntryField(),
            _taxTypeRow(),
            _companyPhoneEntryField(),
            _submitButton(),
          ],
        ),
      ),
    );
  }

  Text _titleBanner() {
    return const Text(
      "Join us as an organizer",
      style: TextStyle(
        fontSize: 22,
        fontWeight: FontWeight.w600,
        color: Colors.blueAccent,
      ),
    );
  }

  Row _taxTypeRow() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: <Widget>[
        Flexible(
          flex: 3,
          child: TaxTypeDropdown(setTaxType: setTaxType),
        ),
        const Spacer(),
        Flexible(
          flex: 7,
          child: _companyTaxIdEntryField(),
        ),
      ],
    );
  }

  Future<void> submitOrganizerData() async {
    if (_formKey.currentState!.validate()) {
      Organizer organizer = Organizer(
          _companyName.text,
          '${_addressStreet.text}/${_addressZip.text}/${_addressCity.text}',
          taxType,
          _taxId.text,
          _displayName.text,
          credentials.email,
          credentials.password,
          _phone.text);
      try {
        await Auth().registerOrganizer(organizer);
      } catch (e) {
        Navigator.of(context).popUntil((route) => route.isFirst);
        await showDialog(
          context: context,
          builder: (context) {
            return AlertDialog(
              title: const Text("Error"),
              content:
                  Text("Couldn't sign up, error message:\n${e.toString()}"),
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
        return;
      }
      Navigator.of(context).popUntil((route) => route.isFirst);
      await showDilogAfterRegistration();
    }
  }

  Future<void> showDilogAfterRegistration() async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Thank you"),
          content: const Text("Administrators have received your form. "
              "We will try to verify your data as soon as possible."),
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
        title: const Text("Register as an organizer"),
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

class TaxTypeDropdown extends StatefulWidget {
  final void Function(TaxType) setTaxType;

  const TaxTypeDropdown({super.key, required this.setTaxType});

  @override
  State<TaxTypeDropdown> createState() => _TaxTypeDropdownState();
}

class _TaxTypeDropdownState extends State<TaxTypeDropdown> {
  List<String> list = TaxType.values.map((e) => e.name).toList();
  late String dropdownValue;
  late void Function(TaxType) setTaxType;

  @override
  void initState() {
    dropdownValue = list.first;
    setTaxType = widget.setTaxType;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return DropdownButton<String>(
      value: dropdownValue,
      icon: const Icon(Icons.expand_more),
      style: const TextStyle(fontSize: 14),
      onChanged: (String? value) {
        // This is called when the user selects an item.
        setState(
          () {
            dropdownValue = value!;
            setTaxType(TaxType.values.firstWhere((e) => e.name == value));
          },
        );
      },
      items: list.map<DropdownMenuItem<String>>(
        (String value) {
          return DropdownMenuItem<String>(
            value: value,
            child: Text(value),
          );
        },
      ).toList(),
    );
  }
}
