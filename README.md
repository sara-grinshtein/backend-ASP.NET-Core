# 🚗 Project Yedidim

<a id="overview"></a>
## 🌍 Overview
Project Yedidim is a backend system built with **ASP.NET Core**.  
It manages requests from people who need help (**Helpeds**) and coordinates responses from volunteers (**Volunteers**).  
Assignments are computed automatically using a **graph-based matching algorithm**. ⚡

<a id="roles"></a>
## 👥 Roles
- 🧑‍🦽 **Helped**  
  A person who submits a request for assistance (creates a *Message*).  

- 🙋 **Volunteer**  
  A person who can be assigned to one or more requests.  

> 🔑 There is no "Admin" role.  
> ⚙️ Matching is determined by the algorithm, with the option for a volunteer to confirm or adjust suggestions.

<a id="algorithm"></a>
## 🔀 Dispatch Algorithm
The dispatching system is modeled as a **flow network**:

```mermaid
flowchart LR
    S((Source))
    M1[Message 1]
    M2[Message 2]
    V1[Volunteer 1]
    V2[Volunteer 2]
    V3[Volunteer 3]
    T((Sink))

    S --> M1
    S --> M2

    M1 --> V1
    M1 --> V2
    M2 --> V2
    M2 --> V3

    V1 --> T
    V2 --> T
    V3 --> T
```

✨ Key points:  
- Each **Message** can be assigned to ➕ multiple Volunteers.  
- Each **Volunteer** usually handles ➡️ one request at a time.  
- The algorithm finds the optimal set of assignments using **Max-Flow** 📈.  

<a id="sequence"></a>
## 📜 Flow Example

```mermaid
sequenceDiagram
    participant H as Helped
    participant M as Message
    participant A as Algorithm
    participant V as Volunteers

    H->>M: 📝 Submit request
    M->>A: 📊 Added to graph
    A->>V: 🤖 Suggest assignments
    V->>A: ✅ Confirm/adjust
    A->>H: 🤝 Volunteers assigned
```

<a id="tech-stack"></a>
## 🛠️ Tech Stack
- ⚡ **ASP.NET Core 7.0**  
- 🗄️ **Entity Framework Core**  
- 🧩 **SQL Server**  
- 🖼️ **Mermaid.js** for diagrams

<a id="conclusion"></a>
## 🎯 Conclusion
Project Yedidim provides a structured way to match people in need with volunteers,  
leveraging a graph-based algorithm with flexibility for one-to-many assignments.  
❤️ Making help more efficient and impactful.
