import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/organizer_account.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';
import 'package:ticketer/backend_communication/model/user.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/common/event_tile.dart';
import 'package:ticketer/pages/login/page_organizer_register.dart';
import 'package:ticketer/pages/user/user_drawer.dart';

class OrganizerDataEdit extends StatefulWidget {
  const OrganizerDataEdit({Key? key}) : super(key: key);

  @override
  State<OrganizerDataEdit> createState() => _OrganizerDataEditState();
}

class _OrganizerDataEditState extends State<OrganizerDataEdit> {
  final TextEditingController _companyName = TextEditingController();
  final TextEditingController _addressCity = TextEditingController();
  final TextEditingController _addressZip = TextEditingController();
  final TextEditingController _addressStreet = TextEditingController();
  final TextEditingController _taxId = TextEditingController();
  final TextEditingController _displayName = TextEditingController();
  final TextEditingController _phone = TextEditingController();
  final _formKey = GlobalKey<FormState>();

  late TaxType taxType = TaxType.NIP;
  void setTaxType(TaxType t) {
    taxType = t;
  }

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
      "Hello orgnizer, edit data",
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
    );
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
      OrganizerAccount organizer = OrganizerAccount(
          _companyName.text,
          '${_addressStreet.text}/${_addressZip.text}/${_addressCity.text}',
          taxType,
          _taxId.text,
          _displayName.text,
          "",
          "",
          _phone.text);
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
