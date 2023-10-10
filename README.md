# V2ex.MAUI

[![CI Build](https://github.com/rwecho/V2ex.Maui/actions/workflows/ci.yml/badge.svg)](https://github.com/rwecho/V2ex.Maui/actions/workflows/ci.yml) [![PUBLISH Build](https://github.com/rwecho/V2ex.Maui/actions/workflows/publish.yml/badge.svg)](https://github.com/rwecho/V2ex.Maui/actions/workflows/publish.yml)



I believe this project might seem unnecessary since there are already many excellent apps that everyone is familiar with. However, with the upcoming release of .NET Core 8.0 and the growing interest in the .NET community, I want to seize this opportunity to establish this project. Currently, it remains a rough prototype with only a few basic features, far from being fully functional and user-friendly. But I trust that with a better understanding of app development, I will shape this app into a multi-platform, user-friendly, and feature-rich V2ex application.

## Screenshots

<div style="display: flex; justify-content: space-around;margin:10px">
<img src="./docs/screenshots/Screenshot_1689848332.png"  width="25%"  alt="tvOS screenshot" />
<img src="./docs/screenshots/Screenshot_1689848360.png"  width="25%" alt="tvOS screenshot" />
<img src="./docs/screenshots/Screenshot_1689848386.png"  width="25%" alt="tvOS screenshot" />


</div>
<div style="display: flex; justify-content: space-around;margin:10px">
<img src="./docs/screenshots/Screenshot_1689848397.png"  width="25%" alt="tvOS screenshot" />
<img src="./docs/screenshots/Screenshot_1689848416.png"  width="25%" alt="tvOS screenshot" />
</div>


## Download (AppCenter.ms)
<div style="text-align:center">
<img src="data:image/png;base64,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"/>
</div>

## Features

__basic functions__

* Login
* Tab topics
* Topic
* My Favorites
* Settings


## Contributes

Welcome everyone to contribute and participate! If you need a specific feature, you can start by submitting an [issue](https://github.com/rwecho/V2ex.MAUI/issues) for discussion. Alternatively, you are also encouraged to [fork](https://github.com/rwecho/V2ex.MAUI/fork)  the repository, make changes, and submit a pull request (PR).


<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/rwecho"><img src="https://avatars.githubusercontent.com/u/8048753?v=4?s=100" width="100px;" alt="rwecho"/><br /><sub><b>RWECHO</b></sub></a><br /><a href="https://github.com/rwecho/V2ex.Maui/commits?author=rwecho" title="Code">💻</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

For details please visit [insights](https://github.com/rwecho/V2ex.Maui/graphs/contributors)

## Acknowledges

- [V2ex](https://www.v2ex.com)

- [V2er](https://github.com/v2er-app/Android)

    V2er is my daily app.

    > V2er is a mobile app for the V2EX website, a community-driven forum that discusses a wide range of topics such as technology, programming, and lifestyle.
- [Abp](https://github.com/abpframework/abp)

    > ABP is an open-source application framework for ASP.NET Core that helps developers create modular and maintainable applications.

- [MAUI](https://github.com/dotnet/maui)
- [CommunityToolkit/Maui](https://github.com/CommunityToolkit/Maui)
- [HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack)
## License
The source code is licensed under MIT. License is available [here](./LICENSE.txt)