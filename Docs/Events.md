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
      <code>
        {<br/>
        EventId : string<br/>
        CreationDate : DataTime<br/>
        Id : string<br/>
        Email : string<br/>
        FirstName : string<br/>
        LastName : string<br/>
        }
      </code>
    </td>
    <td>CreateUserEvent</td>
  </tr>
  <tr>
    <td>UpdateUserEvent</td>
    <td>
      <code>
        {<br/>
        EventId : string<br/>
        CreationDate : DataTime<br/>
        Id : string<br/>
        Email : string<br/>
        FirstName : string<br/>
        LastName : string<br/>
        }
      </code>
    </td>
    <td>UpdateUserEvent</td>
  </tr>
  <tr>
    <td>DeleteUserEvent</td>
    <td>
      <code>
        {<br>
        EventId : string<br/>
        CreationDate : DataTime<br/>
        Id : string<br/>
        }
      </code>
    </td>
    <td>DeleteUserEvent</td>
  </tr>
</table>
