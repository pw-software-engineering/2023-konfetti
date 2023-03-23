import 'package:flutter/material.dart';
import 'package:ticketer/auth/auth.dart';

PreferredSizeWidget ticketerAppBar(String text) {
  return AppBar(
    title: Text(text),
    actions: [
      IconButton(
        onPressed: () => Auth().logOut(),
        icon: const Icon(
          Icons.logout,
          color: Colors.white,
        ),
      )
    ],
  );
}
