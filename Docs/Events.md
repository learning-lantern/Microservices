# User Events

## Exchanges

<table>
  <tr>
    <th>Exchange Name</th>
    <th>Exchange Type</th>
  </tr>
  <tr>
    <td>LearningLantern</td>
    <td>Direct Exchange</td>
  </tr>
</table>

## Queues

<table>
  <tr>
    <th>Queue Name</th>
    <th>Binding To (Routing Keys)</th>
  </tr>
  <tr>
    <td>auth</td>
    <td>
      <ul>
        <li>UserEvent</li>
        <li>DleteUserEvent</li>
      </ul>
    </td>
  </tr>
  <tr>
    <td>chat</td>
    <td>
      <ul>
        <li>newRoom</li>
        <li>joinRoom</li>
      </ul>
    </td>
  </tr>
</table>

## Events

<table>
  <tr>
    <th>Event Name</th>
    <th>Massage Body</th>
    <th>Routing Key</th>
    <th>Send When</th>
    <th>Notes</th>
  </tr>
  <tr>
    <td>UserEvent</td>
    <td>
        {<br>
        Id : string<br>
        FirstName : string<br>
        LastName : string<br>
        }<br>
    </td>
    <td>UserEvent</td>
    <td>User Signup or Update has name</td>
  </tr>
  <tr>
    <td>DeleteUserEvent</td>
    <td>
        {<br>
        Id : string<br>
        }<br>
    </td>
    <td>DeleteUserEvent</td>
    <td>User Delete has account</td>
  </tr>
  <tr>
    <td>newRoom</td>
    <td>
        {<br>
        classId : string<br>
        userId : string<br>
        }<br>
    </td>
    <td>newRoom</td>
    <td>Create a new Room</td>
    <td>userId is Id for the User who Create the room</td>
  </tr>
  <tr>
    <td>joinRoom</td>
    <td>
        {<br>
        classId : string<br>
        userId : string<br>
        }<br>
    </td>
    <td>joinRoom</td>
    <td>When a new User join a Room</td>
  </tr>
</table>
