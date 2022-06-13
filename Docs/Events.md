# User Events

## Exchanges

<table>
  <tr>
    <th>Exchange Name</th>
    <th>Exchange Type</th>
  </tr>
  <tr>
    <td>LearningLantern.EventBus</td>
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
    <td>Users</td>
    <td>
      <ul>
        <li>CreateUserEvent</li>
        <li>UpdateUserEvent</li>
        <li>DleteUserEvent</li>
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
  </tr>
  <tr>
    <td>CreateUserEvent</td>
    <td>
        {<br>
        EventId : string<br/>
        CreationDate : DataTime<br>
        Id : string<br>
        Email : string<br>
        FirstName : string<br>
        LastName : string<br>
        }<br>
    </td>
    <td>CreateUserEvent</td>
  </tr>
  <tr>
    <td>UpdateUserEvent</td>
    <td>
        {<br>
        EventId : string<br>
        CreationDate : DataTime<br>
        Id : string<br>
        Email : string<br>
        FirstName : string<br>
        LastName : string<br>
        }<br>
    </td>
    <td>UpdateUserEvent</td>
  </tr>
  <tr>
    <td>DeleteUserEvent</td>
    <td>
        {<br>
        EventId : string<br>
        CreationDate : DataTime<br>
        Id : string<br>
        }<br>
    </td>
    <td>DeleteUserEvent</td>
  </tr>
</table>
