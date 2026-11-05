# suiss UI blocks catalog

Research for the Richie Design Program (WP1-2). Scope: the *blocks* of the suiss UI library, with a deep dive on blocks useful for the account settings area. The component and theme guide is a separate document (`suiss-ui-guide.md`).

## Method and confidence

- Source of truth: the shadcn-format registry at `https://suiss.app/ui/r/base-suiss/registry.json` (813 items) plus each block's `https://suiss.app/ui/r/base-suiss/{name}.json`. All class strings and structure below were **read from the block source** (`files[0].content`). Raw downloads are in `.design/wp1-2/` (git-ignored).
- The site's rendered pages were not fetched. Visual impressions (how a block looks) are therefore **inferred from the source**, and are marked "(inferred)" where they matter.
- Registry dependency prefix is `@esuiss/` (for example `@esuiss/sidebar`); it is dropped in the tables below.
- Registry item counts: 54 page blocks + 70 chart blocks (`registry:block`), 69 `ui`, 30 `component`, plus 581 `example` items. Everything in this catalog is a `registry:block`, except the pattern components called out in section 3.
- The registry only lists direct dependencies. Blocks also import things the `registryDependencies` field does not list, notably `IconPlaceholder` (see "Gotchas").

## Gotchas when adopting blocks

1. **`IconPlaceholder`**: every block imports `@/app/(create)/components/icon-placeholder` and passes five icon-library names (`lucide=`, `tabler=`, `hugeicons=`, `phosphor=`, `remixicon=`). That file is part of the shadcn "create" app and is **not** a registry dependency. Replace each use with a direct icon import (we pick one library) after copying.
2. **Sample data is inline.** Every block is self-contained client code (`"use client"`, `useState`, fake data, `setTimeout`). Treat blocks as layout and markup references, not as drop-in components.
3. **Base UI, not Radix.** Evidence: `render={<RowLink />}` props on `Item`, `data-open`/`data-panel-open` attributes, `createToastManager`, `ToggleGroup value={[...]}` as arrays, `Select onValueChange` returning `string | null`. Our wiring must follow Base UI conventions.
4. **Settings pages in blocks are admin/phone demos with fake brand copy** ("suiss Account", "Jamie Rivera"). Copy must be rewritten.

---

## 1. Full block inventory

### 1.1 Page blocks (54)

| Name | Category | Description | Registry deps |
|---|---|---|---|
| bag-checkout | bag | Three-step checkout (delivery, payment, review) beside an order summary | button, label, radio-group |
| bag-configure | bag | Product configurator for model, finish, storage with live price | button, label, radio-group, toggle-group |
| bag-review | bag | Shopping bag review with quantity steppers, removable lines, totals | button, stepper |
| bento-closer-look | bento | Auto-advancing accordion of features with matching cross-fading media | accordion |
| bento-features | bento | Bento grid of feature tiles (photos, gradient figures, short claims) | (none) |
| bento-stats | bento | Dark bento grid of headline stats, photo tile, animated charging bar | progress |
| chat-conversations | chat | Messaging layout: conversation list, selected thread, composer | avatar, bubble, button, input, message, message-scroller, split-view |
| chat-support | chat | Support landing section with floating chat panel, quick replies, launcher | bubble, button, input, message, message-scroller |
| chat-thread | chat | Single message thread in a phone frame with reactions and simulated replies | avatar, bubble, button, input, message, message-scroller |
| dash-analytics | dash | App analytics dashboard: metric switcher, interactive area chart, regions | chart, item, sidebar, toggle-group |
| dash-health | dash | Health summary: pinned metric cards, sparklines, category views | avatar, button, sidebar |
| dash-store | dash | Store admin overview: sales stats, bar chart, filtered list | avatar, chart, item, sidebar, toggle-group |
| empty-bag | empty | Empty shopping bag with sign-in and shop actions above recommendations | button, empty |
| empty-offline | empty | Offline empty state with retry that reconnects and reveals an album grid | button, empty, spinner |
| empty-search | empty | Search page: live suggestions, no-results state, suggested queries | button, empty |
| footer-compact | footer | Narrow footer: breadcrumb, collapsible link directory, legal lines | accordion, breadcrumb |
| footer-directory | footer | Full footer: footnotes, breadcrumb, five-column directory, legal | accordion, breadcrumb |
| footer-services | footer | Footer with service highlights band, newsletter signup, directory, legal | input-group |
| grid-accessories | grid | Filterable accessories grid with category chips and add-to-bag | button, toggle-group |
| grid-carousel | grid | Horizontally scrolling product-card carousel with prev/next | carousel |
| grid-lineup | grid | Product lineup comparing models side by side with swatches and prices | button, toggle-group |
| hero-cinematic | hero | Dark centered product hero that changes with the selected model | button, toggle-group |
| hero-product | hero | Centered launch hero: eyebrow, headline, two actions, wide image | button |
| hero-tiles | hero | Pair of promotional hero tiles with title, tagline, two actions | button, utils |
| inbox-categories | inbox | Mail inbox sorted into Primary/Transactions/Updates/Promotions with reader | avatar, button, split-view, textarea, toggle-group |
| inbox-mail | inbox | Three-column mail client: mailboxes, searchable list, reader | avatar, button, input-group, split-view, textarea |
| inbox-support | inbox | Support ticket inbox: status views, ticket list with priorities, detail pane | avatar, button, split-view, textarea, toggle-group |
| log-activity | log | Admin activity log grouped by day, type filter, expandable before/after diffs | button, collapsible, select, sidebar |
| log-live | log | Live admin activity feed that streams events, session stats, pause control | avatar, button, sidebar |
| log-security | log | Security overview: score ring, severity counts, resolvable alerts | button, sidebar, toggle-group, utils |
| login-account | login | Two-step sign-in: email, then password revealed | avatar, button, checkbox, input, label, spinner |
| login-passkey | login | Passwordless sign-in with simulated face-scan dialog then welcome | avatar, button, dialog |
| login-sheet | login | Mobile welcome screen with "Sign in with suiss" bottom sheet | button, label, radio-group, spinner |
| nav-categories | nav | Store landing header with draggable category carousel | carousel, toggle |
| nav-global | nav | Sticky global nav bar with mega-menu panels, search, bag | button, navbar |
| nav-local | nav | Sticky product sub-navigation that highlights the section in view | button, navbar |
| price-bundle | price | Dark subscription bundle picker comparing three plans | button, label, radio-group, utils |
| price-care | price | Device protection plan picker with chips, payment options, add-on | button, label, radio-group, switch, toggle-group |
| price-storage | price | Cloud storage plan picker with monthly/yearly toggle, four plan cards | button, label, radio-group, toggle-group |
| roles-editor | roles | Role editor: roles list beside grouped permission switches, save/revert | button, item, sidebar, split-view, switch |
| roles-matrix | roles | Permission matrix with a checkbox per permission and role, grouped | button, checkbox, sidebar |
| roles-requests | roles | Access request queue: approve/deny, approval length, empty state | avatar, button, empty, select, sidebar |
| settings-account | settings | Account settings: profile sidebar plus Sign-In and Security editable cards | avatar, button, input, sidebar, switch |
| settings-device | settings | Phone-style Settings: search, account row, grouped rows with switches | avatar, input-group, item, switch |
| settings-notifications | settings | Per-app notification settings with live lock-screen preview | item, select, switch, toggle, toggle-group |
| signup-form | signup | Full account creation form: name, country, birthday, email, password rules | button, input, native-select |
| signup-steps | signup | One-question-at-a-time signup with progress indicator | button, input |
| signup-trial | signup | Free trial signup: promo image panel beside short form | button, checkbox, input, label |
| users-detail | users | User directory: member list beside detail pane (profile, sessions, security) | avatar, button, item, select, sidebar, split-view, switch, toggle-group |
| users-invite | users | Invite form: email chips, role, pending invitations list | button, item, select, sidebar, switch, toast |
| users-table | users | Searchable users table with role filter, row selection, bulk actions | avatar, button, checkbox, input-group, sidebar, table, toggle-group |
| verify-code | verify | Two-factor six-digit code entry with error feedback and resend | input-otp |
| verify-device | verify | Trusted-device sign-in: instructions plus simulated approval alert and code | input-otp, spinner |
| verify-email | verify | Check-your-inbox screen after signup with open mail, resend, cooldown | button |

### 1.2 Chart blocks (70, not relevant to settings)

All have registry deps `card` + `chart` (a few add `toggle`, `toggle-group`, `switch`). Seven families of ten: `area-chart-*` (default, linear, step, gradient, axes, stacked, stacked-expand, range, sparkline, interactive), `bar-chart-*` (default, multiple, stacked, horizontal, labeled, negative, active, range, hourly, interactive), `line-chart-*` (default, linear, step, dots, labeled, multiple, goal, goal-dots, forecast, interactive), `pie-chart-*` (default, donut, donut-text, legend, label, separated, nested, half, active, interactive), `radar-chart-*` (default, dots, multiple, legend, lines-only, radius-axis, grid-circle, grid-filled, grid-none, interactive), `radial-chart-*` (default, label, tinted, stacked, text, progress, gauge, rings, batteries, interactive), `tooltip-chart-*` (default-dot, line, dashed, no-indicator, no-label, custom, formatter, icons, label-formatter, interactive).

### 1.3 Adjacent pattern components (`registry:component`, relevant to blocks)

Not blocks, but blocks lean on them and several matter for settings. Full list: action-bar, action-menu, app-shell, command-palette, confirm, context-actions, date-picker, flyout, modal, navbar, paged-list, panel, picker, preview, search-picker, segment-picker, split-view, table-list, trail. Relevant ones are described in section 3.

---

## 2. Account-settings blocks: deep dive

Shared building blocks used throughout (read from `item.json`, `button.json`, `empty.json`):

- `ItemGroup variant="inset"`: `gap-0 overflow-hidden rounded-2xl bg-muted`, items forced `rounded-none`, separators `my-0 ms-4 w-auto` (inset from the left edge by 1rem). This is the "iOS grouped list" and the dominant settings idiom.
- `Item size="sm"`: `gap-3.5 px-3.5 py-3`, `text-base`, flex row, wraps. `ItemContent` is `flex-1 flex-col gap-1`; `ItemTitle` is `text-base font-semibold line-clamp-1` (blocks override with `font-normal`); `ItemDescription` is `text-sm text-muted-foreground line-clamp-2`; `ItemActions` is `flex items-center gap-2`; `ItemMedia variant="icon"` is a 16px icon slot.
- Buttons are pill-shaped (`rounded-full`). Sizes: `xs h-6`, `sm h-8 px-3`, default (about h-9), `lg h-11 px-5.5`. Variants include `secondary`, `plain` (text-primary, no fill), and `destructive` (tinted: `bg-destructive/10`, `text-destructive`, never a solid red fill).

### 2.1 `settings-account` (the primary account-settings reference)

What it is: sidebar with a profile header (avatar, name, email) plus a content pane showing "Sign-In & Security" as a stack of rounded grey cards. Only the Security section is implemented; others show a placeholder line.

**Skeleton**

```
SidebarProvider
 ├─ NavSidebar (Sidebar collapsible="offcanvas")
 │   ├─ SidebarHeader: Avatar size-18, name text-2xl semibold tracking-tight, email text-sm muted
 │   ├─ SidebarContent > SidebarGroup > SidebarMenu > SidebarMenuItem > SidebarMenuButton (icon + label)
 │   └─ SidebarRail
 └─ SidebarInset
     └─ div.mx-auto.flex.w-full.max-w-170.flex-col.gap-3.5.px-5.py-8.md:px-9
         ├─ div.flex.items-center.gap-2: SidebarTrigger(-ms-2) + h1.text-4xl.font-semibold.tracking-tight
         └─ section content (cards, gap-3.5)
```

**Key measurements**

- Content column: `mx-auto w-full max-w-170` (42.5rem = 680px), `gap-3.5`, `px-5 py-8 md:px-9`.
- Page title: `h1 text-4xl font-semibold tracking-tight` (title is the active nav label).
- Card (static/toggle row): `rounded-[1.125rem] bg-muted px-5 py-4.5`, inner `flex items-center gap-3`; text block `flex flex-1 flex-col gap-0.75`; title `text-lg font-semibold`; value `text-base text-muted-foreground`.
- Action affordance for a row is a text link `Edit ›` in `text-base text-primary hover:underline` (not a button). Switch or status text (`text-sm font-semibold`, green) sits at the right end.
- Inline edit: the card expands via `grid grid-rows-[0fr] -> data-open:grid-rows-[1fr]` (`duration-400`, `cubic-bezier(0.32,0.72,0,1)`), revealing `Input h-12 rounded-xl text-lg` plus `Button size="sm"` Save and `variant="outline"` Cancel in `flex gap-2.5` under `pt-3.5`. Container is `inert` while closed.
- Save feedback: a `role="status"` line under the stack, `min-h-6 text-sm`, with a green check and "{Title} saved." (fades in via `starting:` utilities). No toast.
- Sidebar mobile behaviour: `useSidebar().isMobile` closes the drawer on selection. The trigger is always rendered in the page header.

**Excerpt: static toggle row (passkeys)**

```tsx
<div className="flex items-center gap-3 rounded-[1.125rem] bg-muted px-5 py-4.5">
  <div className="flex flex-1 flex-col gap-0.75">
    <label htmlFor={`${idPrefix}-passkeys`} className="text-lg font-semibold">Passkeys</label>
    <p className="text-base text-muted-foreground">{passkeys ? "On for 3 devices" : "Off"}</p>
  </div>
  <Switch id={`${idPrefix}-passkeys`} checked={passkeys} onCheckedChange={setPasskeys} />
</div>
```

**Excerpt: page frame**

```tsx
<SidebarInset>
  <div className="mx-auto flex w-full max-w-170 flex-col gap-3.5 px-5 py-8 md:px-9">
    <div className="flex items-center gap-2">
      <SidebarTrigger className="-ms-2" />
      <h1 className="text-4xl font-semibold tracking-tight">{current?.label}</h1>
    </div>
    ...
```

**Copy style**: sentence fragments, no trailing period on titles ("Passkeys", "Two-factor authentication"), one short muted sentence for description ("Codes are sent to your trusted devices."), state as value text ("On for 3 devices"), confirmation with period ("Email & phone saved."). Nav labels are title case ("Sign-In & Security").

**Composes**: Sidebar family, Avatar, Input, Button, Switch.

**Verdict**: best starting point for the whole account area shell and the Sign-in & security page. Observations: it is the only block with a centered max-width content column; it uses cards (`rounded-[1.125rem]`) where other settings blocks use `ItemGroup inset` (`rounded-2xl`). Radii differ slightly (18px vs 16px); pick one.

### 2.2 `users-detail` (sessions and security rows)

What it is: admin sidebar + `SplitView` (member list left, detail right) with three sub-sections for the selected user: Profile, Sessions, Security. Mostly useful for its detail-pane contents.

**Skeleton**: `SidebarProvider > Sidebar + SidebarInset > SplitView(list, detail)`. Detail pane is `md:p-6`, inner stack `flex flex-col gap-5`; header row `flex items-center gap-3.5` with `Avatar size-16`, name `text-2xl font-bold`, meta `text-sm text-muted-foreground`; section switcher is a `ToggleGroup spacing={0}` with `ToggleGroupItem size="sm" className="px-4"` (a segmented control, not tabs); content `flex flex-col gap-3`.

**Profile rows**: `ItemGroup variant="inset"` with rows `Item size="sm"`: label in `ItemTitle font-normal`, value in `ItemActions className="text-muted-foreground"`; editable row swaps the value for a `Select size="sm" className="h-7.5"` (`SelectContent align="end"`). Label-left, value-right layout, separators between each.

**Sessions list (directly reusable)**

```tsx
<ItemGroup variant="inset" className="**:data-[slot=item-separator]:ms-15">
  <Item size="sm" role="listitem">
    <ItemMedia className="size-7.5 rounded-[7px] bg-zinc-500 text-white">{icon}</ItemMedia>
    <ItemContent className="gap-0">
      <ItemTitle className="font-normal">{session.device}</ItemTitle>
      <ItemDescription className="text-xs">{session.place} · {session.time}</ItemDescription>
    </ItemContent>
    <ItemActions>
      {index === 0 ? <span className="text-sm">This device</span>
        : <Button size="sm" variant="secondary">Sign Out</Button>}
    </ItemActions>
  </Item>
</ItemGroup>
{sessions.length > 1 && (
  <Button size="sm" variant="destructive" className="self-start">Sign Out All Other Sessions</Button>
)}
```

Patterns: icon tile `size-7.5 rounded-[7px]` (about 30px, squircle-ish app-icon tile); separator indented to `ms-15` so it starts after the icon tile; current session shows plain text "This device" instead of a button; per-row action is `secondary` `sm`; bulk action is `destructive sm self-start` below the list; the bulk button disappears when only the current session remains. Description line is `Place · Time` with a middle dot.

**Security rows**: `ItemGroup inset` of label+Switch items ("Two-factor authentication", "Require passkey", "Sign-in alerts"), then a `destructive` button "Send Password Reset Link". Note the destructive variant is used for an action that is not actually destructive (reset link), so do not copy that choice blindly.

**Responsive**: `SplitView` becomes a push navigation stack on mobile (pattern component; list then detail with a back link `min-h-11 text-primary`). Detail has `md:p-6`, list `md:px-4 md:pt-5`. The block adds `SidebarTrigger` row `md:hidden` above.

**Composes**: Sidebar, SplitView, Avatar, Item family, Select, Switch, ToggleGroup, Button.

**Verdict**: source for the Sessions/Devices list and for the Workspaces list (member list pattern), not for the page shell.

### 2.3 `settings-notifications` (preferences)

What it is: per-app notification settings (iOS-like) with a live preview panel on the right.

**Skeleton**: `section.grid.grid-cols-1.md:grid-cols-[minmax(0,1fr)_18.75rem]` -> left column `flex flex-col gap-5 px-5 py-6 md:px-8`, right column preview `bg-muted p-5` (hidden-feel on mobile: it stacks below, inferred; no `hidden` class is used, it simply stacks).

- Section headings: `h2 -mb-3 px-1 text-sm font-semibold` placed *above* each inset group (pulled down with `-mb-3` to tighten against the list since the parent gap is `gap-5`).
- Rows: `ItemGroup variant="inset"` > `Item size="sm"`, title `text-base font-normal`, control right: `Switch`, `Select size="sm" className="bg-transparent px-1 text-muted-foreground"` (value shown as muted text with chevron, borderless look), or `ToggleGroup spacing={0}` with two `size="sm"` items ("Temporary", "Persistent").
- Master switch "Allow Notifications" sits in its own group; the dependent groups are wrapped in `inert={!allow}` with `data-disabled:opacity-35` (`transition-opacity duration-300`).
- Conditional row (Banner Style) appears only when its parent toggle is enabled, preceded by `ItemSeparator`.
- Page header: icon tile `size-10 rounded-[0.625rem]` + `h1 text-2xl font-bold tracking-tight` + `p text-sm text-muted-foreground` ("Notifications").

**Excerpt**

```tsx
<ItemGroup variant="inset">
  <Item size="sm" role="listitem">
    <ItemContent>
      <ItemTitle className="text-base font-normal">
        <label htmlFor={`${idPrefix}-sounds`}>Sounds</label>
      </ItemTitle>
    </ItemContent>
    <ItemActions>
      <Switch id={`${idPrefix}-sounds`} checked={sounds} onCheckedChange={setSounds} />
    </ItemActions>
  </Item>
```

The label is a real `<label htmlFor>` inside `ItemTitle` (good accessibility pattern, copy it). The lock-screen preview is app-specific; we would drop it or replace it with an email-style preview (our decision).

**Composes**: Item family, Switch, Select, Toggle, ToggleGroup.

**Verdict**: use for the Preferences page (switch rows, select rows, section headings, master/dependent disabling).

### 2.4 `settings-device`

What it is: iOS "Settings" home screen inside a phone frame (the frame is demo chrome; ignore it). Useful for the **mobile settings index**: search field, an account row, and grouped link rows with colored icon tiles.

- Title: `h1 px-1 text-4xl font-bold tracking-tight`; search is `InputGroup h-9 rounded-[0.625rem]` with leading icon addon, `type="search"`.
- Groups are `ItemGroup variant="inset" className="bg-background dark:bg-muted **:data-[slot=item-separator]:ms-14.25"` (here the inset group sits on a muted page background, so the group is `bg-background`, the inverse of the other blocks).
- Account row: `Item size="sm"` rendered as a link (`render={<RowLink href="#account" />}`), `Avatar size-14.5`, title `text-xl font-semibold`, description `text-sm`, trailing chevron `size-3.5 text-muted-foreground rtl:rotate-180`.
- Link row: `Item className="min-h-11 gap-3.5 py-1.5"`, `ItemMedia size-7.25 rounded-[7px] bg-(--tint) text-white` (tint via CSS variable), title `text-base font-normal`, right side `ItemActions gap-1.5 text-base text-muted-foreground` with detail text and chevron; toggle rows swap the chevron for a `Switch`.
- Search filter hides empty groups; empty result text `p-7.5 text-center text-base text-muted-foreground` "No Results".
- Wrapper `no-scrollbar flex flex-1 flex-col gap-4.5 overflow-y-auto px-4 pt-6 pb-6`.

**Verdict**: use as the **mobile account-settings index** (list of sub-pages with chevrons); the desktop equivalent is the sidebar in `settings-account`. Note `Item` accepts `render` to become an `<a>`.

### 2.5 Sidebar / app-shell blocks

- `settings-account`, `users-*`, `roles-*`, `log-*`, `dash-*` all repeat a local `NavSidebar` function (about 80 lines): `Sidebar collapsible="icon"` (or `"offcanvas"` in settings-account) > optional `SidebarHeader` (hidden when collapsed via `group-data-[collapsible=icon]:hidden`), `SidebarGroup`s with optional `SidebarGroupLabel`, `SidebarMenuButton isActive tooltip aria-current="page"`, optional `SidebarMenuBadge`, `SidebarFooter`, `SidebarRail`. Item shape `{ value, label, icon, badge?, group? }`.
- Admin blocks put the brand as `p.flex.items-center.gap-2.px-2.pt-1.text-sm.font-semibold` with `<span className="font-bold">suiss</span> Admin` in the header.
- Content pane for admin blocks (not centered, no max width): `flex flex-col gap-5 px-5 py-5.5 md:px-7` with header `flex items-start gap-3` containing `SidebarTrigger -ms-2 mt-0.5`, then `h1 text-3xl font-semibold tracking-tight` and `p text-sm text-muted-foreground` subtitle.
- **`app-shell` (pattern component, not a block)**: a ready-made version of NavSidebar plus a mobile branch. Props: `items` (`{value,label,icon,href?,badge?,group?,disabled?}`), `value/defaultValue/onValueChange`, `header`, `footer`, `variant` (`sidebar|floating|inset`), `collapsible` (`offcanvas|icon|none`), `side`, `rail`, `render` (link component, default `<a>`), `label`. On mobile (`useSidebar().isMobile`) it replaces the sidebar with a floating `TabBar` (items plus a "More" overflow trigger) and pads `main` with `pb-[calc(--spacing(16)+env(safe-area-inset-bottom))]`. Exports `AppShell`, `AppShellTrigger` (returns null on mobile). Deps: sidebar, tab-bar, utils. **This is the better base than copying NavSidebar.** Caveat: a mobile tab bar suits top-level app navigation; for a seven-item settings list we likely want a settings index screen on mobile instead (see recommendations).

### 2.6 Members, lists with row actions, tables

**`users-table`**: header row `flex flex-wrap items-start gap-3` (trigger, title block `flex-1`, search `InputGroup h-8 flex-1 sm:w-48`, `Button size="sm"` "Invite"; the search and button wrap onto their own full-width line on mobile via `w-full sm:w-auto`). Role filter is a `ToggleGroup` with `max-w-full overflow-x-auto` and items `px-4 md:px-8`. Table wrapper `overflow-hidden rounded-xl bg-muted`; header cells `text-xs font-normal text-muted-foreground`; columns collapse: Role `hidden sm:table-cell`, Last sign-in `hidden md:table-cell`; zebra via `even:bg-background/60`; selected row becomes `bg-primary text-primary-foreground` (iOS-style selection, not checkbox-forward, the Checkbox is `sr-only`); status text colour only (Suspended `text-destructive`, Invited muted). Footer bar `flex min-h-12 flex-wrap items-center gap-2 border-t border-border py-2 ps-4 pe-3` with live-region selection count and bulk buttons (secondary, secondary, destructive). Empty: `p-6 text-center text-sm text-muted-foreground` "No matching users." Verdict: workable for a workspace members list in the *workspace* admin area; for account settings (a person's own workspaces) a list is more apt than a table.

**`users-invite`**: single-column form frame; first row is `Item size="sm" className="items-start"` with a fixed label column `ItemContent className="w-22.5 flex-none pt-1"` and an email chip input on the right (`span` chips `h-6.5 rounded-md bg-secondary ps-2.5 pe-0.5 text-sm` with an `icon-xs` remove button; the raw `<input>` is `min-w-35 flex-1 bg-transparent text-sm outline-none`). Summary line left (`flex-1 text-sm text-muted-foreground`, `aria-live="polite"`) and primary `Button size="sm"` right. Success feedback is a **toast** (`createToastManager`, `type: "success"`). "Pending invitations" list: `h2 -mb-2 px-1 text-sm font-semibold`, then inset `ItemGroup` with `ItemMedia size-7 rounded-[7px] bg-zinc-500`, title + `ItemDescription text-xs` "Role · 2 days ago", actions `Resend` (secondary sm) and `Cancel` (destructive sm). Empty state is one muted line: "No pending invitations." Use for invitations (workspace invites an account has received would be the same shape with Accept/Decline).

**`roles-requests`**: `ul.overflow-hidden.rounded-2xl.bg-muted` of custom `li` rows (avatar `size-8.5 mt-3` + text column + actions); action pair `Deny` (secondary sm) and `Approve` (default sm) right-aligned on `sm+`, stacked below the text on mobile (`flex-col sm:flex-row`); row removal animates via `grid-rows-[1fr] -> data-closing:grid-rows-[0fr]`; result replaces the buttons with a `role="status"` word ("Approved" green, "Denied" `text-destructive`). Empty state uses the `Empty` component: `Empty className="gap-3 p-10"`, `EmptyMedia` (a 40px green icon), `EmptyTitle text-xl` "No pending requests." plus an `EmptyContent` link. Good reference for a "pending invitations to workspaces" list and for the empty state.

**`roles-editor`, `roles-matrix`**: permission-switch editor and checkbox matrix. Not account settings (skim only); relevant only if we build workspace roles later.

**`log-activity`, `log-security`, `log-live`**: `log-activity` is a day-grouped feed: `section.flex.flex-col.gap-2` > `h2 px-1 text-sm font-semibold` (day) > `ul.overflow-hidden.rounded-2xl.bg-muted` with rows `min-h-11 gap-3 py-2 ps-4 pe-4`, colored `size-7 rounded-[7px]` icon tile, `text-sm` with bold actor, `text-xs text-muted-foreground` detail, `tabular-nums` time, and a `Collapsible` expanding to a `dl` Before/After diff (`ps-14`). Candidate for a **security activity / sign-in history** list. `log-security` shows an alerts list: `ul.overflow-hidden.rounded-2xl.bg-muted`, rows `relative flex min-h-13 items-center gap-3 px-4 py-2.5`, hairline separators drawn with `not-first:before:` pseudo-elements inset by `start-14.5`, a severity tile (red/orange/yellow, resolved turns green), title `text-sm font-semibold truncate`, detail `text-xs text-muted-foreground`, trailing `Resolve` (default sm) or `Reopen` (secondary sm). Its score ring header (`rounded-xl bg-muted px-4 py-3.5`, `size-13.5` SVG ring) is a nice "account security checkup" summary idea (inferred fit).

### 2.7 Passkey, verification and sign-in blocks (auth screens, reuse for step-up and enrolment)

These are full-screen centered flows, not settings pages, but the copy and layout are reusable for "Add a passkey", "Confirm it's you" step-up and email change verification.

- **Frame (all of login/verify/signup)**: `section.flex.min-h-150.items-center.justify-center.p-6` with a column `flex flex-col items-center text-center`; h1 `text-4xl font-semibold tracking-tight text-balance md:text-5xl` (`mt-5` below a `size-16`/`size-18` `rounded-full bg-muted text-primary` icon disc), lead `text-lg`/`text-xl text-muted-foreground` with `max-w-110`/`max-w-120`, `text-balance`.
- **`login-passkey`**: icon disc `size-18`, h1 "Sign in without a password.", lead "With a passkey, your face or fingerprint is all it takes. Nothing to remember, nothing to type.", identity pill `rounded-full bg-muted py-2 ps-2 pe-4` (avatar `size-7`, email `text-base`, `Change ›` link `text-sm text-primary`), CTA `Button size="lg" className="mt-5.5 gap-2"` "Continue with passkey", secondary link "Sign in with password ›" (`mt-4.5 text-sm text-primary`), fine print `mt-10 max-w-105 text-xs`. The scan Dialog: `DialogContent showCloseButton={false} w-65 rounded-3xl bg-popover/82 backdrop-blur-2xl`, title and description centered, `Button variant="plain"` Cancel. The success state shows the same centered column with avatar and "Welcome, Jamie." Copy ends titles with a period; trailing `›` marks text links.
- **`verify-code`**: 6-digit `InputOTP` rendered as two groups of three (`InputOTPGroup gap-2.5`, `flex gap-4` between), each slot `h-14 w-11 rounded-xl sm:h-15.5 sm:w-12.5 text-3xl`, status via `group-data-[status=error]/code:border-destructive` and `...ok:border-green-500`; status line `mt-3.5 min-h-5 text-sm` (`aria-live`); resend row `mt-6 flex flex-col items-center gap-2.5 text-sm` with a `tabular-nums` countdown. Directly relevant because Richie's Duende OTP is 8-char base32 (our format differs; slot count and group split must change).
- **`verify-device`**, **`verify-email`**: trusted-device approval and check-your-inbox screens (same frame; `verify-email` has open-mail and resend with cooldown). Reuse for "Confirm your new email".
- **`login-account`**: grouped single-border credential field (one `rounded-xl border border-input` container with stacked borderless `h-14 text-lg` inputs separated by `border-t`), shake animation on error, error line `mt-2.5 min-h-5.5 text-sm text-destructive`, form width `w-115 max-w-full`.
- **`signup-form`**: the form-field convention: `fieldset` per group with `legend mb-2 text-sm font-semibold`, the grouped-border input container (`h-13 rounded-none bg-transparent px-4 text-lg`, `has-focus-visible:ring-3`, `data-invalid:border-destructive data-invalid:bg-destructive/5`), hint `mt-1.5 text-xs text-muted-foreground`, password-rules checklist (`size-4` ring dots turning green), submit `Button size="lg" className="mt-3 min-w-40 self-center"`, container `mx-auto flex w-115 max-w-full flex-col text-center` on `px-6 py-9`. Reuse for password-less "Change email" and profile forms if we want the grouped-field look (our app has no passwords, so ignore the password parts).

### 2.8 Destructive confirmation and "danger zone"

**There is no danger-zone block and no delete-account block in suiss.** The closest building blocks:

- **`confirm` (pattern component)**: `Confirm`, `ConfirmTrigger`, `ConfirmContent`, `ConfirmHeader`, `ConfirmFooter`, `ConfirmTitle`, `ConfirmDescription`, `ConfirmAction`, `ConfirmCancel`. Renders as an `AlertDialog` on desktop and an `AlertSheet` (iOS action sheet) on mobile, switching on `useIsMobile()`. On mobile it hoists footer buttons into the sheet group and renders Cancel separately; `dismissible` defaults to `false` (tapping the backdrop does not dismiss). Deps: alert-dialog, alert-sheet, use-mobile. Use for "Sign out all other sessions", "Leave workspace", "Remove passkey", "Delete account".
- Destructive styling in blocks: tinted, never solid: `Button variant="destructive"` = `bg-destructive/10 text-destructive` (`dark:bg-destructive/20`). One block (users-invite "Cancel") layers a darker text colour `text-[color-mix(in_oklab,var(--destructive),var(--foreground)_20%)]` for contrast; status text uses `text-destructive`; error fields use `border-destructive bg-destructive/5`.
- Placement of destructive actions in blocks: bulk/sign-out-all goes **below** the list as `self-start` (left-aligned, not full width); row-level destructive is a trailing `sm` button in `ItemActions`; there is no heading, border or colored card around them (no "danger zone" styling).

### 2.9 Empty states

- `empty` UI component (`Empty`, `EmptyHeader`, `EmptyMedia`, `EmptyTitle` (`text-lg font-semibold`), `EmptyDescription`, `EmptyContent`).
- `empty-search`: left-aligned variant (`items-start gap-3 rounded-none p-0 text-start`, title `text-3xl tracking-tight`, description `text-lg/normal`, secondary suggestion buttons `h-8.5 px-3.5`); `empty-offline`, `empty-bag` are marketing-flavoured, centered.
- In settings blocks, empty lists are mostly **a single muted line** rather than the `Empty` component: `p-6 text-center text-sm text-muted-foreground` ("No matching users."), `p-7.5 text-center text-sm text-muted-foreground` ("Nothing here yet."), or `px-1 text-sm text-muted-foreground` ("No pending invitations."). `roles-requests` is the only one using `Empty` with an icon (`size-10`), `EmptyTitle text-xl`, `Empty p-10 gap-3`.

---

## 3. Pattern components that matter for settings (not blocks)

| Component | Behaviour | Use for |
|---|---|---|
| `app-shell` | Sidebar on desktop, floating tab bar with "More" on mobile | Outer account app frame |
| `split-view` | 2/3-column list/detail, push navigation stack on mobile with `text-primary` back link | Workspaces list to detail, sessions detail |
| `confirm` | AlertDialog desktop, action sheet mobile | All destructive confirmations |
| `action-menu` | Dropdown on desktop, action sheet on mobile | Per-row overflow menus |
| `modal` | Dialog on desktop, bottom drawer on mobile | Add passkey, rename device |
| `panel` | Sheet on desktop, swipeable drawer on mobile | Row detail panels |
| `table-list` | Table on desktop, grouped list on mobile | Members tables |
| `paged-list` | Numbered pagination desktop, "Load more" mobile | Long activity lists |
| `segment-picker`, `picker` | Segmented control / custom Select on desktop, native select on mobile | Preferences |
| `undo-bar`, `swipe-actions`, `tip` | Undo toast, swipe-to-reveal row actions, dismissible tips | Remove passkey (undo), mobile session rows |

(Descriptions from registry `description` fields; structure of components not individually inspected except `app-shell` and `confirm`, `item`, `split-view` class names.)

---

## 4. Patterns summary

| Aspect | Convention observed |
|---|---|
| Page width | Only `settings-account` centers a column: `mx-auto w-full max-w-170 px-5 py-8 md:px-9`. Admin blocks are full-width inside `SidebarInset` with `px-5 py-5.5 md:px-7`. Auth/verify screens use `max-w-105`-`max-w-140` centered, form `w-115 max-w-full`. |
| Page title | Settings page `h1 text-4xl font-semibold tracking-tight`; admin pages `h1 text-3xl font-semibold tracking-tight` + `text-sm text-muted-foreground` subtitle; auth heroes `text-4xl md:text-5xl`; phone-style home `text-4xl font-bold`. Detail names `text-2xl font-bold`. |
| Section headings | `h2 px-1 text-sm font-semibold` sitting above a list (no divider, no card), pulled tight with `-mb-2` / `-mb-3` when the parent gap is `gap-5`. |
| Section spacing | Page stack `gap-5` (admin) or `gap-3.5` (settings-account cards); list-to-heading `gap-2`; small control groups `gap-2.5`. |
| Row pattern | Label left, control or value right, in `Item size="sm"` (`px-3.5 py-3`, `min-h-11`); title `text-base font-normal`; secondary line `text-xs`/`text-sm text-muted-foreground`; separators inset (`ms-4`, `ms-14`, `ms-15` after an icon tile). Controls: `Switch`, `Select size="sm" h-7.5`, `ToggleGroup size="sm"`, text `Edit ›`. |
| Icon tiles | `size-7`-`size-7.5 rounded-[7px]` coloured tile with white glyph (app-icon look), `size-10 rounded-[0.625rem]` for page icon. |
| Containers | Card/row surfaces are `bg-muted` (grey, no border, no shadow) with `rounded-2xl` (inset groups, lists, tables use `rounded-xl`/`2xl`) or `rounded-[1.125rem]` (settings-account cards). Borders are only used on inputs (`border-input`) and `border-t border-border` footers. |
| Forms | Inline edit expands inside the card (grid-rows animation) with Save (default sm) + Cancel (outline sm) left-aligned; full forms use grouped-border inputs `h-13 text-lg rounded-xl`, `fieldset`/`legend text-sm font-semibold`, helper `text-xs muted`, error `text-sm text-destructive` in a reserved `min-h-5` live region. |
| Action placement | Row actions right-aligned inside the row (`sm` buttons: `secondary` for neutral, default for positive, `destructive` for removal); list-level destructive/bulk action below the list, `self-start`; primary submit of a form is right-aligned next to a left summary line, or centered `lg` in auth forms. Text links use `text-primary hover:underline` with a trailing `›`. |
| Destructive | Tinted `variant="destructive"` buttons (no solid red), `text-destructive` for state words, `bg-destructive/5` on invalid containers; confirmations via `Confirm` (dialog desktop, sheet mobile); no dedicated danger-zone section. |
| Feedback | Inline `role="status"`/`aria-live="polite"` lines and state words ("Approved") by default; Base UI toast only in `users-invite`. |
| Responsive | Sidebar collapses to an off-canvas drawer via `SidebarTrigger` below the md breakpoint; `md:` is the main breakpoint, `sm:` for control wrapping; tables hide columns (`hidden sm:table-cell`, `hidden md:table-cell`); `SplitView` becomes a push stack; toggle groups scroll horizontally (`overflow-x-auto`). `use-mobile` hook drives pattern components (a JS breakpoint, not just CSS). |
| Motion | Everything fades/slides in with `starting:translate-y-1.5 starting:opacity-0 transition-[opacity,translate] duration-700/800/1000 ease-[cubic-bezier(0.32,0.72,0,1)]` and `motion-reduce:transition-none`. Height reveals use the grid-rows 0fr to 1fr trick. |
| Copy | Short sentence-case labels; headings end with a period only in hero/auth screens; confirmations are "{Thing} saved."; secondary navigation links end with `›`; muted line uses a middle dot to join facts ("San Francisco, US · 2 hr ago"). Title Case is used on action buttons ("Sign Out All Other Sessions", "Send Invites") and some nav labels, sentence case elsewhere, so Richie should choose one. |
| A11y | `label htmlFor` inside `ItemTitle`; `aria-current="page"` on active nav; `inert` on collapsed regions; `aria-live` summaries; `sr-only` checkboxes inside selectable rows. |

---

## 5. Recommendations

| Account page | Start from | Notes |
|---|---|---|
| **Account shell and nav** | `app-shell` component + `settings-account` frame | Use `AppShell` (or `NavSidebar`) with `SidebarHeader` carrying avatar, name, email as in `settings-account` (`collapsible="offcanvas"`). Content `max-w-170` column. On mobile, replace the tab bar with the `settings-device` index (grouped link rows + chevrons) and push to sub-pages (inferred; `app-shell` itself would give a tab bar). |
| **Profile** | `settings-account` `EditableCard` + `signup-form` field styling | Cards with inline edit (name, display name, avatar, email). Avatar uploader is not in suiss; compose from `Avatar` + `Button`. For fields that need validation, use the `signup-form` grouped-border input and error line. Email change should chain to `verify-email`/`verify-code`. |
| **Sign-in & security (passkeys, sessions)** | `settings-account` Security (cards, passkey switch), `users-detail` Sessions list, `login-passkey` copy/dialog, `verify-code` | Passkeys: an inset `ItemGroup` of passkey rows (icon tile, name, "Added {date} · Last used {date}", trailing `action-menu` for rename/remove) plus "Add a passkey" as `Button variant="plain"` or a last row; enrolment via `modal` using the `login-passkey` dialog look. Sessions: `users-detail` list verbatim ("This device", `Sign Out` secondary sm, bulk `destructive` below). Sign-in history: `log-activity` rows. Optional "security checkup" header from `log-security`'s ring. Step-up confirmation: `verify-code` frame with our OTP length. |
| **Workspaces** | `users-detail` list pattern, `roles-requests` and `users-invite` pending list | Inset `ItemGroup` of workspaces (avatar/logo tile, name, "Role · N members", trailing Open/Leave); pending invitations as a second group with `Accept` (default sm) / `Decline` (secondary sm) like `Resend`/`Cancel`; "Create workspace" (limit one) as a row/button with a disabled state and explanatory description. `split-view` only if we want a detail pane. Leave workspace uses `confirm`. |
| **Preferences** | `settings-notifications` | Section `h2`s over inset groups; switches for email notification types, selects for language/time zone/theme (`Select size="sm"` muted borderless look), `ToggleGroup` for density/theme. Master toggle that disables dependants via `inert` + `opacity-35`. Drop the phone preview. |
| **Connected apps** | `users-detail` Sessions list shape or `settings-device` link rows | Row: app icon tile, name, "Connected {date} · scopes", trailing `Revoke` (destructive sm) or `action-menu`. Empty state: `Empty` as in `roles-requests` ("No connected apps."). No OAuth-consent block exists; the Duende consent screen would be composed from the auth frame. |
| **Delete account** | `confirm` + own section | Compose: a quiet section at the bottom of Security or its own page: `h2`, one muted paragraph on consequences (what happens to workspaces), `Button variant="destructive"` `self-start`, opening `Confirm` (desktop dialog, mobile sheet) with a typed-confirmation `Input` (not provided by suiss). Follow suiss's restrained destructive styling (tinted, no red banner). |

### What suiss lacks (we compose ourselves)

- Danger zone / delete-account section and typed-confirmation dialog.
- Avatar/photo upload and crop; display-name or handle fields.
- Passkey management list (rename, remove, "last used"), WebAuthn enrolment states and error handling (only a simulated face scan exists).
- Sessions with revoke confirmations and geo/IP formatting (the sample is static strings).
- Connected apps / OAuth grants and consent screen.
- Workspace list with switcher, leave workspace, create-workspace limit messaging.
- A shared settings page shell (heading + section + row primitives): every block hand-rolls these (`NavSidebar` is copy-pasted in each admin block). We should extract `SettingsPage`, `SettingsSection` (h2 + inset group), `SettingsRow` (label, description, control) once.
- Persistent save feedback strategy (inline status vs toast is inconsistent across blocks), form validation with a form library, loading/skeleton states (none observed), and error states for failed saves.
- Localisation: the blocks are English-only strings, though they do use logical properties (`ms-`, `ps-`, `inset-e-`, `rtl:rotate-180`), which is a plus.

### Open questions / unverified

- Rendered appearance and exact pixel sizes (for example `size-18`, `max-w-170`) are taken from Tailwind class names; this repo's Tailwind configuration must support the custom spacing scale (fractional `0.75`, `4.5`, `7.25` etc. imply Tailwind v4 with a continuous spacing scale).
- Block pages on `suiss.app/ui` were not fetched, so any docs-page-only guidance (usage notes, install commands via `npx shadcn add @esuiss/...`) is not captured.
- `registryDependencies` for `roles-editor`, `roles-matrix`, `log-live`, `signup-steps`, `signup-trial`, `verify-device`, `empty-*`, and the layout blocks outside settings were read from the registry listing only; their source was not reviewed except where noted above (`roles-requests`, `log-*`, `verify-code`, `login-*`, `signup-form`, `users-*`, `settings-*`, `empty-search`, `app-shell`, `confirm`).
