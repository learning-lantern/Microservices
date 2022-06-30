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
</table>

## Events

<table>
  <tr>
    <th>Event Name</th>
    <th>Massage Body</th>
    <th>Routing Key</th>
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
  </tr>
  <tr>
    <td>DeleteUserEvent</td>
    <td>
        {<br>
        Id : string<br>
        }<br>
    </td>
    <td>DeleteUserEvent</td>
  </tr>
</table>
