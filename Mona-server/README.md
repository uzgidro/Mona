# MONA server part

____

## HTTP methods

### Auth

____

#### Sign Up

Request

https://siteurl/auth/sign-up (POST)

```json
{
  "username": "string",
  "firstName": "string",
  "lastName": "string",
  "password": "string"
}
``` 

Response

- 200 if success
- 409 if given __username__ exists
- 400 if something gone wrong

#### Sign In

Request

https://siteurl/auth/sign-in (POST)

```json
{
  "username": "string",
  "password": "string"
}
``` 

Response

```json
{
  "accessToken": "string",
  "refreshToken": "string"
}
   ```

- 400 if username or password incorrect

### Send message with files

____

Append Bearer token to header. Use form-data to send data, __first__ append messageRequest in JSON format
to form-data name "message", then append files to form-data name "file".

https://siteurl/message/send (POST)

Request example

``` ts
let formData = new FormData();
formData.append('message', JSON.stringify(messageRequest));
const filesArr = [...this.selectedFiles];
filesArr.forEach(file => {
  formData.append("file", file, file.name);
});
this.http.post(siteurl/message/send, formData);
```

Response example

Message dto

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

- 400 if something went wrong: read exception

### Download files

____
Append Bearer token to header.

https://siteurl/files/download/{id} (GET)

```ts
const headers = new HttpHeaders();
headers.set('Accept', 'application/octet-stream');
this.http.get(siteurl / files / download / file.id, {headers: headers, responseType: 'blob'})
    .subscribe((response: Blob) => {
        const downloadLink = document.createElement('a');
        const url = window.URL.createObjectURL(blob);
        downloadLink.href = url;
        downloadLink.download = filename;
        document.body.appendChild(downloadLink);
        downloadLink.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(downloadLink);
    });
```

## Hub methods

### sendMessage

Create new message, send direct to user or to group.

Request

Message request

```json
{
  "Id": "string",
  "Text": "string",
  "ReceiverId": "string",
  "ChatId": "string",
  "ReplyId": "string",
  "ForwardId": "string",
  "CreatedAt": "string (ISO 8601 format)"
}
```

Response

Message dto to ReceiveMessage listener

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

Chat dto to UpdateChat listener

```json
{
  "chatId": "string",
  "chatName": "string",
  "message": "string",
  "messageTime": "string (ISO 8601 format)",
  "receiverId": "string",
  "senderId": "string",
  "senderName": "string",
  "isForward": "boolean",
  "chatIcon": "string"
}
```

### editMessage

Edit created message

Request

Message request

```json
{
  "Id": "string",
  "Text": "string",
  "ReceiverId": "string",
  "ChatId": "string",
  "ReplyId": "string",
  "ForwardId": "string",
  "CreatedAt": "string (ISO 8601 format)"
}
```

Response

Message dto to ModifyMessage listener

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

### pinMessage

Request

```
"messageId": "string"
```

Response

Message dto to PinMessage listener

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

### deleteMessageForMyself

Request

```
"messageId": "string"
```

Response
to DeleteMessage listener

```
"id": "string"
```

### deleteMessageForEveryone

Request

```
"messageId": "string"
```

Response
to DeleteMessage listener

```
"id": "string"
```

### getUsers

Response to Invoke method

```json
{
  "id": "string",
  "name": "string",
  "chatId": "string",
  "icon": "string"
}
```

### getChats

Response to Invoke method

```json
{
  "id": "string",
  "name": "string",
  "chatId": "string",
  "icon": "string"
}
```

### getMessagesByChatId

Request

```
"chatId": "string"
```

Response to Invoke method

```json
{
  "id": "string",
  "name": "string",
  "chatId": "string",
  "icon": "string"
}
```

Response to Invoke method

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

## Hub listeners

### ReceiveMessage

Append to collection/db/etc.

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

### ModifyMessage

Edit message in collection/db/etc.

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

### DeleteMessage

Delete from collection/db/etc.

```
"messageId": "string"
```

### PinMessage

Set field pin = true to collection/db/etc.

```json
{
  "id": "string",
  "senderId": "string",
  "senderName": "string",
  "chatId": "string",
  "receiverId": "string",
  "receiver": "string",
  "message": "string",
  "files": [
    {
      "id": "string",
      "name": "string",
      "size": "long",
      "path": "string"
    }
  ],
  "forward": {
    "creatorId": "string",
    "creatorName": "string"
  },
  "reply": {
    "id": "string",
    "replyTo": "string",
    "repliedMessage": "string"
  },
  "isPinned": "boolean",
  "isEdited": "boolean",
  "createdAt": "string (ISO 8601 format)"
}
```

### ReceiveMessage

Read the exception.

### RemoveChat

Remove from collection/db/etc

```
    "chatId": "string"
```

### UpdateChat

Update chat list

```json
{
  "chatId": "string",
  "chatName": "string",
  "message": "string",
  "messageTime": "string (ISO 8601 format)",
  "receiverId": "string",
  "senderId": "string",
  "senderName": "string",
  "isForward": "boolean",
  "chatIcon": "string"
}
```