import 'package:flutter/material.dart';

class DialogContactList extends StatefulWidget {
  @override
  State<DialogContactList> createState() => _DialogContactListState();
}

class _DialogContactListState extends State<DialogContactList> {
  @override
  Widget build(BuildContext context) {
    return Dialog(
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text('This is a typical dialog.'),
            const SizedBox(height: 15),
            TextButton(
              onPressed: () {
                Navigator.pop(context);
              },
              child: const Text('Close'),
            ),
          ],
        ),
      ),
    );
  }
}
