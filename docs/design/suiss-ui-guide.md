# suiss UI guide for Richie

How to design (static mockups) and later build Richie UI with the suiss component library. Written for agents working cold. Block inventory lives in [`suiss-blocks-catalog.md`](./suiss-blocks-catalog.md); this guide covers the component library, the visual language, the theme tokens and how the code is put together.

Evidence: registry JSON for 63 items (`https://suiss.app/ui/r/base-suiss/<name>.json`), the docs as Markdown (`llms.txt`, `theming.md`, component and pattern `.md` pages), four settings blocks (checked only for type and spacing), and this repo's `globals.css` and `button.tsx`. Fetched 2026-11-04. Raw downloads are in `.design/wp1-1/` (git-ignored). Anything not checked is marked **(unverified)**.

---

## 0. The ten rules

1. **Pills and soft rounding.** Buttons are `rounded-full`. Fields use 10 px (`rounded-lg`). Menus and toasts use 12 px. Cards, dialogs, grouped lists, alerts and empty states use 18 px (`rounded-2xl`). Sheets use 28 px.
2. **Fields are filled, not outlined.** Inputs and selects are 44 px tall, with a 4 % black fill (`bg-secondary`), a transparent border and 16 px text. The border only shows up on focus or error.
3. **Only two font weights.** Text is 400 or 600 (semibold). Button labels are regular weight. No suiss component uses `font-medium`. Bold (700) appears only in a few block headings.
4. **Labels are 16 px semibold**, descriptions are 14 px in `muted-foreground`, and errors are 14 px in `destructive`.
5. **Destructive buttons are tinted, not solid.** They have a 10 % red fill with red text. Badges have no background by default; they are just coloured 12 px semibold text.
6. **Settings look like iOS Settings.** Rows sit in a `bg-muted` box with 18 px corners, separated by hairlines that start 16 px in (`ItemGroup`/`FieldGroup variant="inset"`).
7. **Elevation is a shadow plus a 5 % hairline ring.** Cards get `shadow-md`, menus `shadow-lg`, dialogs and sheets `shadow-xl`. The overlay is `bg-black/50`.
8. **One focus ring everywhere:** the border turns `--ring`, plus a 3 px ring of `--ring` at 50 % (`focus-visible:border-ring ring-3 ring-ring/50`).
9. **Icons are Phosphor (regular weight), 16 px inside controls.** Alerts and empty states use 20 px, toasts 24 px, alert-dialog media 32 px.
10. **Primitives are Base UI, not Radix.** Compose with `render={...}`, never `asChild`. State is styled through data attributes (`data-checked`, `data-open`, `data-highlighted`, `data-active`).

---

## 1. What suiss is, and where the truth lives

suiss/ui (by e-suiss, MIT) is a copy-paste component library in the shadcn/ui registry format: 68 React components, 19 patterns, 11 interactions, charts, about 54 page blocks and AI compositions. It also ships React Native, SwiftUI and Compose versions that share the same token names. Components are published for three primitive bases: Base UI (the default), Radix and React Aria. They come in nine "styles"; ours is the `suiss` style on Base UI (`base-suiss`). The visual idea is Apple-like: a white or true-black canvas, grey text "in the system style" and a blue primary. The token hexes match Apple's web palette (#1d1d1f, #0071e3, #f5f5f7, #6e6e73).

| What | URL |
|---|---|
| Docs home | https://suiss.app/ui/docs (append `.md` to any docs URL for Markdown) |
| Agent index / full text | https://suiss.app/ui/llms.txt and https://suiss.app/ui/llms-full.txt (about 8 MB) |
| Component doc | `https://suiss.app/ui/docs/components/<name>` (patterns: `/docs/patterns/<name>`) |
| Theming | https://suiss.app/ui/docs/theming |
| **Registry item, our base and style** | `https://suiss.app/ui/r/base-suiss/<name>.json` (source of truth for classes) |
| Registry index | https://suiss.app/ui/r/base-suiss/registry.json (813 items: 69 ui, 30 patterns/interactions, 124 blocks, 581 examples) |
| Examples | Also registry items, e.g. `field-inset`, `item-with-avatar`, `dialog-destructive`, `alert-dialog-small`, `toast-types` |
| Source | https://github.com/e-suiss/ui (not inspected) |

The `.md` docs show code for the default base with Phosphor icons, which matches ours. The registry **MCP server always reads the Radix base**, so its code samples use `asChild`. Do not paste those into this repo.

---

## 2. Visual spec

### 2.1 Colour tokens (`frontend/packages/ui/src/styles/globals.css`)

Our values match `@esuiss/theme-suiss` exactly (diffed). Hex values are sRGB approximations; use the oklch values in code.

| Token | Light (oklch) | ≈ hex | Dark (oklch) | ≈ hex | Role |
|---|---|---|---|---|---|
| `background` | `1 0 0` | #ffffff | `0 0 0` | #000000 | Page |
| `foreground` | `0.2316 0.0038 286.09` | #1d1d1f | `0.9707 0.0026 286.29` | #f5f5f7 | Body text |
| `card` / `-foreground` | `1 0 0` / fg | #ffffff | `0.2316 0.0038 286.09` / fg | #1d1d1f | Raised panels |
| `popover` / `-foreground` | `1 0 0` / fg | #ffffff | `0.2316 0.0038 286.09` / fg | #1d1d1f | Menus, dialogs, sheets, toasts |
| `primary` | `0.5629 0.1933 256.16` | #0071e3 | `0.669 0.1808 251.84` | #2997ff | Default buttons, checked controls, links, active icons |
| `primary-foreground` | `1 0 0` | #fff | `1 0 0` | #fff | Text on primary |
| `secondary` | `0 0 0 / 4%` | black 4 % | `1 0 0 / 10%` | white 10 % | Fills: secondary buttons, inputs, tracks, tab lists |
| `secondary-foreground` | fg | | fg | | |
| `muted` | `0.9707 0.0026 286.29` | #f5f5f7 | `0.2006 0.002 286.21` | #161617 | Quiet surfaces: grouped lists, filled cards, alerts, skeletons |
| `muted-foreground` | `0.5399 0.0077 286.13` | #6e6e73 | `0.7105 0.0072 286.2` | #a1a1a6 | Descriptions, placeholders, labels in tables and menus |
| `accent` / `-foreground` | `0 0 0 / 6%` / fg | black 6 % | `1 0 0 / 10%` / fg | white 10 % | Hover, highlighted menu items, selected rows |
| `destructive` | `0.5329 0.2255 27.27` | #d0000a | `0.7033 0.2236 28.29` | #ff5447 | Destructive actions, errors |
| `border` | `0.8652 0.0068 286.24` | #d2d2d7 | `0.3802 0.005 286.15` | #424245 | Hairlines and dividers |
| `input` | `0.6217 0.0074 286.17` | #86868b | `0.5399 0.0077 286.13` | #6e6e73 | Unchecked switch track, stronger borders |
| `ring` | `0.5629 0.1933 256.16` | #0071e3 | same | #0071e3 | Focus ring |
| `sidebar` | = muted | #f5f5f7 | = muted | #161617 | Sidebar background |
| `sidebar-accent` | = accent | | = accent | | Sidebar hover and active |
| `sidebar-primary` | = primary | | = dark primary | | Active nav icon |
| `sidebar-border` / `-ring` | = border / ring | | = border / ring | | |
| `chart-1…5` | `0.6028 0.2177 257.42`, `0.6998 0.1109 212.72`, `0.5295 0.191 278.34`, `0.7652 0.1752 62.57`, `0.6483 0.0073 286.18` | | see css | | Charts only |

**Derived roles:**

- **Tinted:** `bg-primary/10` (dark `/20`) with text `color-mix(in oklab, var(--primary) 80%, var(--foreground))`. Used for tinted buttons, combobox chips and pressed toggles.
- **Hover on filled elements:** the same colour at 80 % (`hover:bg-primary/80`).
- **Hover on transparent elements:** `bg-accent`.
- **Disabled:** `opacity-50`.
- **Invalid:** `border-destructive`, `ring-3 ring-destructive/20` (dark `/40`), and on fields `bg-destructive/5` (dark `/10`).

**Status colours are not tokens.** Use the Tailwind palette:

- Success: `text-green-600 dark:text-green-400`, `bg-green-500/10`.
- Warning: `orange-500`.
- Toast icons: success `fill-green-500`, info `fill-primary`, warning `fill-orange-500`, error `fill-destructive`.
- Blocks darken small green text with `text-[color-mix(in_oklab,var(--color-green-500),var(--foreground)_45%)] dark:text-green-500`, because green-600 on white is only 3.2:1.

### 2.2 Typography

- **Font stack:** `--font-sans: -apple-system, BlinkMacSystemFont, "Inter Variable", sans-serif`. That is SF Pro on Apple devices and Inter elsewhere (`@fontsource-variable/inter`). Headings use the same font (`--font-heading: var(--font-sans)`).
- **Sizes and line heights** are the Tailwind defaults. **Weights:** 400 and 600 only. Page titles use `tracking-tight` (−0.025em).

| Class | px / line | Used by |
|---|---|---|
| `text-xs` | 12 / 16 | Badges, kbd, tooltips, menu/select group labels, table headers, sidebar group labels, `xs` buttons |
| `text-sm` | 14 / 20 | Buttons, tab triggers, toggles, descriptions (field, card, dialog), errors, table body, toasts, breadcrumbs |
| `text-base` | 16 / 24 | Inputs, selects, labels (semibold), menu and select items, item titles (semibold), card/dialog/sheet titles (semibold), sidebar items, `lg`/`xl` buttons |
| `text-lg` | 18 / 28 | Alert-dialog title, empty-state title, settings-card row title (block) |
| `text-xl` | 20 / 28 | OTP slot characters |
| `text-2xl` … `text-4xl` | 24/32, 30/36, 36/40 | Page titles in blocks (`text-3xl font-semibold tracking-tight` users-table; `text-4xl` settings-account) |

### 2.3 Spacing, sizing and density

- **Grid:** 4 px (`--spacing: 0.25rem`).
- **Density:** suiss is roomy, at touch-friendly iOS scale. Default fields are 44 px; default buttons are 36 px.

| Element | Height | Padding (x) | Notes |
|---|---|---|---|
| Button `xs` / `sm` / default / `lg` / `xl` | 24 / 32 / 36 / 44 / 56 | 11 / 12 / 16 / 22 / 31 | Icon side tightens: 8 / 8 / 12 / 18 / 24. Gap 4–8. |
| Icon button `icon-xs` … `icon-xl` | 24 / 32 / 36 / 44 / 56 square | | Round |
| Input default / `md` / `sm` | 44 / 36 / 32 | 12 / 12 / 10 | `md`/`sm` use 14 px text; `sm` has an 8 px radius |
| Select trigger default / `sm` | 44 / 36 | 12 | `w-fit` by default |
| Native select | 44 / 36 (`sm`) | ps 12, pe 32 | |
| Textarea | min 64, grows | 12 all sides | |
| Switch default / `sm` | 24 (52 wide) / 20 (44 wide) | | Thumb 20×32 / 16×26 (a pill) |
| Checkbox / radio | 16 (16 wide) | | Hit area extended to about 40×32 |
| OTP slot | 52 (44 wide) | | 8 px gap |
| Tabs list / segmented | 36 (tabs) / 44 (segmented, default items) | p 4, gap 4 | |
| Menu item | 40 (8 px + 24 px line + 8 px) | 12 | |
| Item default / `sm` / `xs` | about 54 / 50 / 46 single-line (includes a 1 px transparent border) | 16×14 / 14×12 / 12×10 | Gap 14 / 14 / 10 |
| Inset field row | min 44 | 16, py 6 | |
| Table header / cell | 48 / about 44 | 12 | |
| Sidebar width / item | 256 (icon mode 48) / 36 | 12 | |
| Avatar `sm`/default/`lg`/`xl` | 24 / 32 / 40 / 64 | | |

**Layout rhythm:**

| Context | Spacing |
|---|---|
| Field internals (label, control, description) | 12 px |
| Between fields (`FieldGroup`) | 28 px |
| `FieldSet` | 24 px |
| Card | 24 px padding and 24 px gap (`sm`: 16) |
| Dialog | 24 px padding and 24 px gap |
| Popover | 16 px padding, 16 px gap |
| Toast content | 16 px padding, 12 px gap |
| Empty state | 48 px padding, 16 px gap |
| Settings page column (block) | `max-w-170` (680 px), `px-5 py-8 md:px-9`, 14 px gap between sections |
| Sectioned page | 20 px between groups; heading 8 px above its group |

### 2.4 Radius

`--radius: 0.625rem` (10 px).

| Class | Formula | px | Used for |
|---|---|---|---|
| `rounded-xs` | ×0.3 | 3 | Link focus outlines, tooltip arrow |
| `rounded-sm` | ×0.5 | 5 | Kbd, `xs` block buttons, sidebar badge |
| (checkbox) | ×0.4 | 4 | Checkbox |
| `rounded-md` | ×0.8 | 8 | Menu and select items, highlight pill, tab and segment indicator, skeleton, `sm` inputs, block buttons |
| `rounded-lg` | ×1 | 10 | Inputs, selects, OTP slots, toggles, empty-media tile |
| `rounded-xl` | ×1.2 | 12 | Menus and select popups, tooltips, toasts, textarea, tab list, segmented container, Item, choice cards |
| `rounded-2xl` | ×1.8 | 18 | Cards, dialogs, alert dialogs, popovers, hover cards, inset grouped lists, alerts, empty states, filled accordion |
| `rounded-3xl` | ×2.8 | 28 | Sheets |
| `rounded-full` | | pill | Buttons, badges, switches, avatars, radios, chips, progress and slider tracks |

Inside `SidebarHeader` and `SidebarContent`, `--radius` is re-set to `radius-xl` (12 px). Nested `rounded-md` therefore becomes 9.6 px.

### 2.5 Elevation

The hairline ring is `ring-1 ring-foreground/5` (dark: `/10`), drawn as a 1 px box-shadow.

| Surface | Shadow (Tailwind v4 defaults) |
|---|---|
| Card | `shadow-md` (`0 4px 6px -1px rgb(0 0 0/.1), 0 2px 4px -2px rgb(0 0 0/.1)`) plus hairline |
| Menu, select, combobox, popover, hover card | `shadow-lg` (`0 10px 15px -3px …/.1, 0 4px 6px -4px …/.1`) plus hairline |
| Dialog, alert dialog, sheet | `shadow-xl` (`0 20px 25px -5px …/.1, 0 8px 10px -6px …/.1`) plus hairline (sheet has no ring) |
| Toast | `0 8px 32px rgb(0 0 0/.14)` plus hairline; dark: `shadow-lg`, no ring |
| Switch and slider thumbs, tab and segment indicator | `shadow-sm` |
| Overlay | `bg-black/50` |

### 2.6 States and focus

| State | Classes |
|---|---|
| Keyboard focus | `focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/50` (destructive button: `ring-destructive/20`, `border-destructive/40`). Menu items do not use a ring; they get `focus:bg-accent`. |
| Hover, filled | `hover:bg-<color>/80` |
| Hover, transparent | `hover:bg-accent hover:text-accent-foreground` |
| Open trigger | `aria-expanded:bg-accent` (outline, ghost and plain buttons) |
| Selected or active | `data-active:bg-sidebar-accent`; table row `data-selected:bg-accent`; toggle `aria-pressed:bg-accent` with tinted text |
| Invalid | `aria-invalid:border-destructive aria-invalid:ring-3 aria-invalid:ring-destructive/20 aria-invalid:bg-destructive/5` |
| Disabled | `disabled:opacity-50` / `data-disabled:opacity-50`, plus `pointer-events-none` or `cursor-not-allowed` |

### 2.7 Motion

| What | Timing |
|---|---|
| Popups (menu, select, popover, tooltip, dialog) | `animate-in fade-in-0 zoom-in-95` plus an 8 px slide from the trigger side, `duration-100` |
| Sheets | 400 ms `cubic-bezier(0.32,0.72,0,1)` (the "iOS ease"), 40 px slide plus fade |
| Indicators (select highlight, tab and segment pill, switch thumb) | 200–300 ms, iOS ease |
| Toasts | 500 ms `cubic-bezier(0.22,1,0.36,1)`; older toasts stack scaled 10 % per step, peeking 12 px |
| Toggle press | `active:scale-96`, spring ease `cubic-bezier(0.3,1.25,0.5,1)` |
| Spinner | 8 spokes, `spin 0.8s steps(8)` |
| Skeleton | `animate-pulse` (2 s) |

Indicators, toasts and the theme switch respect `prefers-reduced-motion`.

### 2.8 Icons

- **Library:** Phosphor (`@phosphor-icons/react`, regular weight).
- **Source format:** components are authored with an `IconPlaceholder` that the CLI rewrites to the icon library set in `components.json`.
- **Default size:** 16 px (`[&_svg:not([class*='size-'])]:size-4`), and 12 px in `xs` buttons and badges.
- **Built-in glyphs:** `Check`, `X`, `CaretUpDown` (select), `CaretDown` (combobox), `CaretRight` (breadcrumb, submenu), `DotsThree`, `Minus` (OTP separator), `MagnifyingGlass`, `Sidebar` (sidebar trigger), `CheckCircle`, `Info`, `Warning`, `XCircle` (toasts).
- **In buttons:** mark the icon with `data-icon="inline-start|inline-end"` to tighten that side's padding.

### 2.9 Dark mode

- **How it switches:** a `dark` class on `<html>` (`@custom-variant dark (&:is(.dark *))`).
- **Surfaces:** the canvas becomes true black; cards and popovers become #1d1d1f; `muted` becomes #161617.
- **Fills:** `secondary` and `accent` become white at 10 %.
- **Controls:** checkbox and radio lose their border and take a `bg-border` fill. The switch thumb becomes `bg-foreground`. The tab and segment indicator becomes `bg-border`.
- **Theme switch:** optional `@esuiss/theme` (ThemeProvider, ThemeToggle, ThemeScript). Do not add it unless a switch is needed.

---

## 3. Component reference

- **Package deps** are those beyond `@base-ui/react`, the `@esuiss/utils` `cn` helper and Phosphor.
- **Look** gives px. Verbatim class strings are in Appendix A.
- Docs for every component: `https://suiss.app/ui/docs/components/<id>`.

### 3.1 Actions

| Component (`@esuiss/…`) | Parts / variants / sizes | Look | Use | Avoid |
|---|---|---|---|---|
| **button** (cva) | `variant`: default, secondary, outline, tinted, ghost, plain, destructive, link. `size`: xs, sm, default, lg, xl, icon-xs … icon-xl. `block`. `render`. `buttonVariants()`. | Pill, 14 px/400, h36 px16. Default: primary fill. Secondary: 4 % black fill. Outline: 1 px border on background. Tinted: primary/10 with primary-mixed text. Ghost: transparent. Plain: primary text. Destructive: red/10 with red text. Block: full width, radius 5–12 by size. | default = the one primary action per view; secondary/outline/tinted = supporting; ghost/plain = low emphasis and toolbars; destructive = delete/remove | More than one `default` button per region; solid-red buttons (not in suiss); `link` for navigation that needs a button target |
| **button-group** (+separator) | `ButtonGroup orientation`, `ButtonGroupText`, `ButtonGroupSeparator` | Joins pills: first and last ends rounded, inner edges square, `border-s-0` | Split buttons, input plus button combos | Unrelated actions |
| **toggle** | `variant` default/outline; `size` sm (32), default (36), lg (40) | rounded-lg, 14 px; pressed `bg-accent` with tinted text; `active:scale-96` | Toolbar on/off (bold, pin) | Settings (use Switch) |
| **toggle-group** (+toggle) | `spacing` (default 2; `0` = attached or segmented), `variant`, `size`, `orientation` | **Segmented** (`spacing={0}`, default variant): `bg-secondary rounded-xl p-1 gap-1`; segments rounded-md; pressed `bg-card shadow-sm` (dark `bg-border`), sliding indicator | Exclusive choice of 2–5 short options (view mode, period). Pattern `segment-picker` turns into a native select on mobile. | Switching page content (use Tabs) |

### 3.2 Form controls

| Component | Parts / variants / sizes | Look | Use | Avoid |
|---|---|---|---|---|
| **input** | `size` default, `md`, `sm`; any native `type` | h44, `bg-secondary`, transparent 1 px border, rounded-lg, px12, 16 px, primary caret, muted placeholder | Short free text | Long text (Textarea), codes (InputOTP), adornments (InputGroup) |
| **textarea** | `resizable="inside"\|"outside"` | rounded-xl, p12, min-h-16, grows with content (`field-sizing-content`); a custom drag handle when resizable | Messages, notes, bios | Single line |
| **input-group** (+button, input, textarea) | `InputGroup`, `InputGroupAddon align=inline-start\|inline-end\|block-start\|block-end`, `InputGroupButton size xs\|sm\|icon-xs\|icon-sm`, `InputGroupText`, `InputGroupInput`, `InputGroupTextarea` | Same shell as input (h44, filled); ring is drawn on the group; addon text is 16 px muted, icons 16 px | Search icon, prefix/suffix, copy or clear button, kbd hint, composer with toolbar | A plain field |
| **label** | | 16 px semibold, flex gap-8; regular weight when wrapping a checkbox, switch or radio | Standalone controls | Inside `Field` (use `FieldLabel`) |
| **field** (+label, separator) | `Field orientation=vertical\|horizontal\|responsive`, `FieldLabel`, `FieldContent`, `FieldTitle`, `FieldDescription`, `FieldError errors=[]`, `FieldGroup variant=default\|inset`, `FieldSet`, `FieldLegend variant=legend\|label`, `FieldSeparator` | Vertical: gap 12; description 14 px muted; error 14 px red (`role=alert`). Group gap 28. **Inset:** `rounded-2xl bg-muted`, rows min-h44 px16 py6, hairlines inset 16 px, labels regular weight, inputs inside become transparent, h32, right-aligned. **Choice card:** a `FieldLabel` wrapping a `Field` gives a rounded-xl bordered card; checked state is `border-primary ring-1 ring-primary`. | Every labelled control. It wires `aria-describedby` automatically. | Display-only rows (use Item) |
| **select** | `Select`, `SelectTrigger size=default\|sm`, `SelectValue`, `SelectContent side/align/alignItemWithTrigger`, `SelectGroup`, `SelectLabel`, `SelectItem`, `SelectSeparator` | Trigger like input (h44, filled), `CaretUpDown` 16 px muted, **`w-fit`**. Popup rounded-xl, min-w-36. Items py8 ps12 pe32, 16 px. Sliding `bg-accent` highlight pill. Primary check at the end. | One value from a fixed list. Pattern `picker` turns into a native select on mobile. | More than about 15 options (Combobox), actions (DropdownMenu) |
| **native-select** | `NativeSelect size`, `NativeSelectOption`, `NativeSelectOptGroup` | Like input, with the caret icon at end-10 px | Mobile or simple forms | Rich option content |
| **combobox** (+button, input-group) | `Combobox items`, `ComboboxInput showTrigger showClear`, `ComboboxContent`, `ComboboxList`, `ComboboxItem`, `ComboboxEmpty`, `ComboboxGroup/Label/Collection/Separator`, `ComboboxChips`, `ComboboxChip`, `ComboboxChipsInput`, `useComboboxAnchor` | Input-group shell; popup like select. Chips: h22, rounded-full, `bg-primary/10`, 12 px tinted text, ghost × button. | Long lists and type-to-filter; multi-select (teammates, tags). Pattern `search-picker`. | Short fixed lists |
| **checkbox** | single part | 16 px, 4 px radius, 1 px `foreground/25` border, faint shadow; checked: primary fill, white 14 px check | Independent options, form submit | Instant settings (Switch) |
| **radio-group** | `RadioGroup` (grid gap-12), `RadioGroupItem` | 16 px circle; checked: primary fill, 8 px white dot (dark 10 px) | 2–5 visible exclusive options | Long lists |
| **switch** | `size` default, sm | Track 52×24 (sm 44×20), 2 px border; off `bg-input` grey #86868b, on primary. Pill thumb 32×20, white, `shadow-sm`, slides 16 px and stretches while pressed. | Settings that apply immediately | Form fields submitted later |
| **input-otp** (npm `input-otp`) | `InputOTP maxLength`, `InputOTPGroup` (gap 8), `InputOTPSlot index`, `InputOTPSeparator` | Slots 44×52, rounded-lg, `bg-secondary`, 1.5 px border (transparent, primary when active), 20 px semibold tabular, blinking primary caret | Verification and 2FA codes. Richie's 8-char base32 codes: two groups of 4. On narrow screens this needs smaller slots (unverified fit). | Other short values |
| **slider** | | Track h4 `bg-border`, primary range, pill thumb 32×20 white with `shadow-md` | Ranges | |

### 3.3 Display

| Component | Parts / variants | Look | Use | Avoid |
|---|---|---|---|---|
| **item** | `Item variant=default\|outline\|muted size=default\|sm\|xs render`, `ItemMedia variant=default\|icon\|image`, `ItemContent`, `ItemTitle`, `ItemDescription`, `ItemActions`, `ItemHeader`, `ItemFooter`, `ItemGroup variant=default\|inset`, `ItemSeparator` | rounded-xl. Default px16 py14 gap14. Title 16 px semibold, one line. Description 14 px muted, 2 lines. Image media 40 px rounded-md. **Inset group:** `rounded-2xl bg-muted`, rows square, separators `ms-4`. A link item hovers `bg-accent`. | Settings rows, member and session lists, any row of media + text + actions | Form fields with validation (Field) |
| **card** | `Card size=default\|sm variant=default\|filled`, `CardHeader`, `CardTitle`, `CardDescription`, `CardAction`, `CardContent`, `CardFooter` | rounded-2xl, white, `shadow-md` plus hairline, 24 px padding and gap. Title 16 px semibold. Filled: `bg-muted`, no shadow. | Self-contained summaries: plan, metric, form section | Dense row lists (Item/Table) |
| **badge** | `variant` default, secondary, destructive, outline, ghost, link; `render` | h20, 12 px semibold, rounded-full. **default/secondary/destructive have no fill and no padding** (coloured text only). outline: 1 px border, px8. ghost: px8. | Status, counts, metadata | Actions |
| **avatar** | `Avatar size=sm\|default\|lg\|xl`, `AvatarImage`, `AvatarFallback`, `AvatarBadge`, `AvatarGroup`, `AvatarGroupCount` | Round 24/32/40/64. Fallback: vertical grey gradient (`muted-foreground` 90→100 %), white semibold uppercase initials 12/12/16/24 px. Badge: primary dot with a 2 px background ring. Group overlaps −8 px with rings. | People, workspaces | |
| **table** | `Table`, `TableHeader`, `TableBody`, `TableFooter`, `TableRow`, `TableHead`, `TableCell`, `TableCaption` | 14 px. Head h48 px12, 12 px semibold muted. Cell p12, tabular numbers. Row `border-b`, hover `bg-accent`. | Records compared in columns. Pattern `table-list` wraps it in a `rounded-2xl border` section with a title row (`px-6 py-4 text-base font-semibold`) and becomes an inset list on mobile. | Fewer than 3 columns (use Item) |
| **separator** | `orientation` | 1 px `bg-border` | Between groups | Inside menus (use their own separator) |
| **kbd** | `Kbd mod`, `KbdGroup` | h22, min-w22, rounded-sm, `bg-muted`, 12 px muted; **hidden below md** | Shortcut hints | The only place a shortcut is documented |
| **skeleton** | | rounded-md, `bg-muted`, pulse; size it yourself | Loading placeholders | Unknown-length waits (Spinner) |
| **spinner** | | 16 px, 8-spoke iOS spinner in `currentColor`, `role=status` | Saving, loading more | |
| **progress** | `Progress`, `ProgressTrack`, `ProgressIndicator`, `ProgressLabel`, `ProgressValue` | Track h4, rounded-full, `bg-muted`; primary fill | Determinate progress | |
| **empty** | `Empty`, `EmptyHeader`, `EmptyMedia variant=default\|icon`, `EmptyTitle`, `EmptyDescription`, `EmptyContent` | Centred, p48, gap16, rounded-2xl, **no border unless you add `border`** (it only has `border-dashed`). Icon tile 40 px rounded-lg `bg-muted` with a 20 px icon. Title 18 px semibold. Description 14 px muted. | No data, no results, first run | |
| **alert** | `Alert variant=default\|destructive`, `AlertTitle`, `AlertDescription`, `AlertAction` | rounded-2xl, p20, `bg-muted`. Leading icon 20 px (primary or destructive). Title 16 px semibold. Description 16 px muted. Action at top-end. | Inline, persistent notices and errors | Transient feedback (Toast) |
| **accordion** | `variant` default/filled | Items `ms-4` with `border-t`; trigger min-h44, 16 px, hover underline; filled: rounded-2xl `bg-muted` | FAQs, advanced settings | |

### 3.4 Overlays

| Component | Parts / props | Look | Use | Avoid |
|---|---|---|---|---|
| **dialog** (+button) | `Dialog`, `DialogTrigger render`, `DialogContent showCloseButton closeLabel`, `DialogHeader`, `DialogTitle`, `DialogDescription`, `DialogFooter showCloseButton`, `DialogClose` | Centred, **max-w-md (448)**, p24 gap24, rounded-2xl, `shadow-xl` plus hairline. Title 16 px semibold; description 14 px muted. Footer right-aligned row (column-reverse on mobile). Close: 32 px round ghost button on `bg-secondary`, 19 px from the top, 16 px from the end. | Short focused tasks (edit 1–4 fields). Pattern `modal` becomes a drawer on mobile. | Long forms (Sheet), confirmations (AlertDialog) |
| **alert-dialog** (+button) | `AlertDialogContent size=default\|sm`, `Header`, `Media`, `Title`, `Description`, `Footer`, `Action variant`, `Cancel variant` (default `secondary`) | **max-w 340 (sm 320)**, centred text. Title 18 px semibold. Footer **2-column grid** of equal buttons. Media: 64 px circle `bg-muted` with a 32 px icon. The overlay does not close it. | Confirm sign-out, delete, discard. Pattern `confirm` becomes an action sheet on mobile. | Anything with inputs |
| **sheet** (+button) | `SheetContent side=right\|left\|top\|bottom showCloseButton closeLabel`, `SheetHeader`, `SheetFooter`, `SheetTitle`, `SheetDescription`, `SheetClose` | **Floating** 8 px from the edges, rounded-3xl (28), `bg-popover shadow-xl`, `w-3/4 sm:max-w-sm` (384). Header and footer p24. 400 ms slide. | Side tasks that keep the page in context: details, filters, longer edit forms. Pattern `panel` becomes a drawer on mobile. | Short confirms |
| **drawer** | `Drawer`, `DrawerContent` (handle, snap points) | Base UI drawer, swipeable, bottom by default | Mobile, via patterns | Desktop-first settings |
| **popover** | `PopoverContent`, `PopoverHeader`, `PopoverTitle`, `PopoverDescription` | w-72 (288), p16 gap16, rounded-2xl, `shadow-lg` | Small interactive panels related to a trigger | Plain hints (Tooltip) |
| **tooltip** | `TooltipProvider delay=0`, `Tooltip`, `TooltipTrigger`, `TooltipContent side` | **Inverted**: `bg-foreground text-background`, 12 px, rounded-xl, px12 py6, arrow, max-w-xs | Naming icon-only buttons, showing shortcuts | Essential or interactive content |
| **hover-card** | | Like popover (w-72, rounded-2xl, p16) | Link previews | Touch-critical info |
| **dropdown-menu** | `DropdownMenuContent align=start side sideOffset=4`, `Item variant=default\|destructive inset`, `CheckboxItem`, `RadioGroup/RadioItem`, `Label`, `Separator`, `Shortcut mod`, `Sub/SubTrigger/SubContent` | min-w-48 (192), p6, rounded-xl, `bg-popover shadow-lg` plus hairline. Items px12 py8, 16 px, rounded-md, gap10, focus `bg-accent`. Destructive item: red text on red/10. Label 12 px semibold muted. Separator `border/50`, full bleed. | Row actions (the ⋯ menu), account menu. Pattern `action-menu` becomes an action sheet on mobile. | Choosing a form value (Select) |
| **context-menu** | Same parts as dropdown | Same | Right-click on rows | Sole access to an action |
| **command** (npm `cmdk`) | Inline or in a dialog | Popover look | Command palette (pattern `command-palette`) | |
| **toast** (Base UI toast, **not sonner**) | `Toaster` (mount once), `toast.add({ title, description, type, timeout, actionProps })`, `createToastManager`, `useToastManager`, `type`: success, info, warning, error, loading | Bottom-centre on mobile, bottom-end from `sm`; max-w-sm (384). rounded-xl, `bg-popover`, soft shadow. Row: 24 px status icon, 14 px semibold title, 14 px muted description, outline `sm` action, ghost × button. Stack scales down 10 % per step and expands on hover. | Short confirmations ("Changes saved"), background results, Undo | Decisions (AlertDialog), errors that need reading (Alert) |

### 3.5 Navigation

| Component | Parts | Look | Use |
|---|---|---|---|
| **sidebar** (+button, input, separator, sheet, skeleton, tooltip, direction, use-mobile, use-platform) | `SidebarProvider` (cookie `sidebar_state`, Cmd/Ctrl+B), `Sidebar side variant=sidebar\|floating\|inset collapsible=offcanvas\|icon\|none`, `SidebarInset`, `SidebarTrigger`, `SidebarRail`, `SidebarHeader/Content/Footer`, `SidebarGroup/GroupLabel/GroupAction/GroupContent`, `SidebarMenu/MenuItem/MenuButton isActive tooltip size=default\|sm\|lg variant=default\|outline`, `SidebarMenuAction showOnHover`, `SidebarMenuBadge`, `SidebarMenuSub*`, `SidebarMenuSkeleton`, `SidebarInput`, `SidebarSeparator` | 256 px wide, `bg-sidebar` (#f5f5f7), end border. Group p8. Label h32 px12, 12 px semibold muted. Menu gap 2. Button h36 px12, 16 px, radius about 9.6, hover and active `bg-sidebar-accent`, active icon `text-sidebar-primary`. `inset`: main content becomes a `rounded-2xl shadow-sm` panel 8 px in. Below 768 px it renders as a Sheet. | App navigation (pattern `app-shell` becomes a tab bar on mobile) |
| **tabs** | `Tabs orientation`, `TabsList variant=default\|line`, `TabsTrigger`, `TabsContent` | Default: `bg-secondary` rounded-xl p4, h36; triggers 14 px muted; active text foreground with a sliding `bg-card shadow-sm` pill. Line: 2 px foreground underline. | Switching views inside a page |
| **breadcrumb** | `Breadcrumb`, `BreadcrumbList`, `BreadcrumbItem`, `BreadcrumbLink render`, `BreadcrumbPage`, `BreadcrumbSeparator`, `BreadcrumbEllipsis` | 14 px muted, gap 12, `CaretRight` 12 px separators, current page in foreground, links underline on hover | Nested settings pages (pattern `trail` becomes a back link on mobile) |
| **pagination** | `Pagination…`, links take Button variants | Active page `secondary`, others `ghost`, `sm`/`icon-sm` | Long tables (pattern `paged-list`) |

Not in suiss for React: chip, text and icon components (mobile only). There is no `form` wrapper (use `<form>` with Field). There is no sonner.

---

## 4. Composition patterns

Each pattern is shown as JSX (exact parts) plus the visual result. Imports in this repo are `@repo/ui/components/ui/<name>`.

### 4.1 Form field: label, description, error

```tsx
<FieldGroup>                                   {/* 28px between fields */}
  <Field data-invalid>                         {/* vertical, gap 12 */}
    <FieldLabel htmlFor="ws-name">Workspace name</FieldLabel>  {/* 16px semibold */}
    <Input id="ws-name" aria-invalid defaultValue="" />        {/* h44 filled; red border/ring/tint when invalid */}
    <FieldDescription>Shown to teammates and customers.</FieldDescription> {/* 14px muted */}
    <FieldError>Enter a workspace name.</FieldError>           {/* 14px destructive, role=alert */}
  </Field>
</FieldGroup>
```

- `FieldError` does **not** set `aria-invalid`. Set it on the control yourself.
- `Field` links its description and error ids into `aria-describedby` automatically.
- For a switch or checkbox beside text, use `<Field orientation="horizontal">` containing `<FieldContent>` (label + description) and then the control.
- Submit row: `<div className="flex gap-2"><Button type="submit">Save</Button><Button variant="secondary">Cancel</Button></div>` (right-aligned in dialogs).

### 4.2 Settings rows (label + control + action)

**A. Inset grouped list (preferences and toggles).** Source: `settings-notifications` block, `item-inset` example.

```tsx
<h2 className="-mb-3 px-1 text-sm font-semibold">Notifications</h2>
<ItemGroup variant="inset">                    {/* rounded-2xl bg-muted, overflow hidden */}
  <Item size="sm" role="listitem">             {/* px14 py12 */}
    <ItemContent>
      <ItemTitle className="text-base font-normal">
        <label htmlFor="email-digest">Email digest</label>
      </ItemTitle>
      <ItemDescription>Daily summary of open conversations.</ItemDescription>
    </ItemContent>
    <ItemActions><Switch id="email-digest" /></ItemActions>
  </Item>
  <ItemSeparator />                            {/* 1px border, starts 16px in */}
  <Item size="sm" role="listitem">
    <ItemContent><ItemTitle className="text-base font-normal">Language</ItemTitle></ItemContent>
    <ItemActions>
      <Select><SelectTrigger size="sm">…</SelectTrigger>…</Select>
    </ItemActions>
  </Item>
</ItemGroup>
```

- Row titles in inset lists use **regular weight** (`font-normal`), like iOS.
- Trailing controls: a Switch, a `sm` Select, a `sm` segmented ToggleGroup, muted value text (`text-base text-muted-foreground`), or a `CaretRight` for drill-in.
- For editable text values in a grouped list, use `FieldGroup variant="inset"` with `Field orientation="horizontal"`. Its inputs automatically become transparent and right-aligned.

**B. Value card with an inline edit (account identity: name, email, password).** Source: `settings-account` block.

- **Card:** a standalone `rounded-2xl bg-muted px-5 py-4.5` box.
- **Left column:** title `text-lg font-semibold`, current value `text-base text-muted-foreground` (3 px gap).
- **Trailing action:** a plain text link, `text-base text-primary hover:underline`, reading "Edit ›".
- **Edit mode:** the card expands (grid-rows transition, 400 ms iOS ease) to an `Input` (the block uses `h-12 rounded-xl border-input bg-background text-lg`), then `Save` (default `sm`) and `Cancel` (outline `sm`).
- **Spacing:** 14 px between cards.
- **After saving:** a `role=status` line shows a check icon and "X saved." in muted green.

**C. Multi-field form section.** Use a `Card` (or a plain section) with `CardHeader` (title and description), `CardContent` holding a `FieldGroup`, and `CardFooter` holding the actions. Use this for workspace profiles and similar.

**D. Danger zone.** Put an inset group or a card containing an `Item` with title, description and `<Button variant="destructive" size="sm">Delete workspace</Button>` in `ItemActions`. Clicking it opens §4.5.

### 4.3 List of items with actions (members, sessions, API keys)

```tsx
<ItemGroup variant="inset">
  <Item role="listitem">
    <ItemMedia><Avatar size="lg"><AvatarImage src alt="Olivia Martin" /><AvatarFallback>OM</AvatarFallback></Avatar></ItemMedia>
    <ItemContent>
      <ItemTitle>Olivia Martin</ItemTitle>                     {/* 16px semibold */}
      <ItemDescription>olivia@acme.com · Admin</ItemDescription>{/* 14px muted */}
    </ItemContent>
    <ItemActions>
      <Badge variant="outline">Pending</Badge>
      <DropdownMenu>
        <DropdownMenuTrigger render={<Button variant="ghost" size="icon-sm" aria-label="Actions for Olivia Martin" />}>
          <DotsThreeIcon />
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end">
          <DropdownMenuItem>Change role</DropdownMenuItem>
          <DropdownMenuSeparator />
          <DropdownMenuItem variant="destructive">Remove from workspace</DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
    </ItemActions>
  </Item>
  <ItemSeparator />
  …
</ItemGroup>
```

- **Many columns or sorting:** use `Table`, or the `table-list` pattern (a `rounded-2xl border` section with a title row).
- **Search above the list:** an `InputGroup` with a `MagnifyingGlass` addon, size `md` if it is in a toolbar.
- **Bulk select:** a checkbox in the first table column (`has-[[role=checkbox]]:pe-0`).

### 4.4 Dialogs

```tsx
<Dialog>
  <DialogTrigger render={<Button variant="secondary" />}>Rename</DialogTrigger>
  <DialogContent>                               {/* 448 max, p24 gap24, close × top-end */}
    <DialogHeader>
      <DialogTitle>Rename inbox</DialogTitle>     {/* 16px semibold */}
      <DialogDescription>Teammates see this name.</DialogDescription>
    </DialogHeader>
    <Field><FieldLabel htmlFor="n">Name</FieldLabel><Input id="n" /></Field>
    <DialogFooter>                              {/* row, right-aligned, gap 8 */}
      <DialogClose render={<Button variant="secondary" />}>Cancel</DialogClose>
      <Button type="submit">Save</Button>
    </DialogFooter>
  </DialogContent>
</Dialog>
```

- **Order:** Cancel first, primary last. The primary sits on the right on desktop; on mobile the order is reversed so the primary is on top.
- **Title:** always include a `DialogTitle` (use `sr-only` if no visible title is wanted).
- **Responsive:** use the `modal` pattern so the dialog becomes a bottom drawer below 768 px.
- **Longer tasks:** use `Sheet` (side `right`) or the `panel` pattern.

### 4.5 Destructive confirmation

**Simple confirm:** AlertDialog, or the `confirm` pattern.

```tsx
<AlertDialog>
  <AlertDialogTrigger render={<Button variant="destructive" />}>Delete workspace</AlertDialogTrigger>
  <AlertDialogContent>                          {/* 340 max, centred */}
    <AlertDialogHeader>
      <AlertDialogMedia><TrashIcon className="text-destructive" /></AlertDialogMedia> {/* optional 64px circle */}
      <AlertDialogTitle>Delete “Acme Support”?</AlertDialogTitle>   {/* 18px semibold */}
      <AlertDialogDescription>All conversations and contacts are removed. This can’t be undone.</AlertDialogDescription>
    </AlertDialogHeader>
    <AlertDialogFooter>                         {/* 2 equal columns */}
      <AlertDialogCancel>Cancel</AlertDialogCancel>                 {/* secondary */}
      <AlertDialogAction variant="destructive">Delete</AlertDialogAction> {/* tinted red */}
    </AlertDialogFooter>
  </AlertDialogContent>
</AlertDialog>
```

- **Async or typed confirmation** (e.g. "type the workspace name"): `AlertDialogAction` closes the dialog immediately, so it cannot wait for a request or validate input. Use `Dialog` instead, with a `Field` and a plain `<Button variant="destructive">` that stays disabled until the input matches. This follows the suiss `dialog-destructive` example.
- **While the request runs:** disable both buttons and put a `Spinner data-icon="inline-start"` in the destructive one. Close the dialog on success, then show a toast.
- **Wording:** title as a question that names the object, the consequence in the description, and the verb on the button ("Delete", not "OK").

### 4.6 Toasts

```tsx
// once, near the app root:  <Toaster>{app}</Toaster>
toast.add({ type: "success", title: "Changes saved" })
toast.add({ type: "error", title: "Couldn’t save", description: "Check your connection and try again." })
toast.add({ title: "Member removed", actionProps: { children: "Undo", onClick: undo } })
```

- **Titles:** at most about 4 words. The description is optional.
- **Undo:** offer it for reversible destructive actions instead of a confirmation, where safe.
- **Duration:** `timeout: 0` keeps a toast open. The default timeout is not documented; Base UI uses 5000 ms **(unverified)**.
- **Reachability:** never put the only path to an action in a toast.

### 4.7 Loading, empty and error states

| State | Recipe |
|---|---|
| Initial load | `Skeleton` shapes that match the real layout (avatar `size-8 rounded-full`, lines `h-4 w-40`, rows inside the same inset group), with `aria-busy="true"` on the region. Sidebar: `SidebarMenuSkeleton`. |
| Action in progress | Disable the button and add `<Spinner data-icon="inline-start" />` with a label such as "Saving…". For background work, use `toast.add({ type: "loading" })` and update it when done. |
| Empty | `<Empty>` (add `className="border"` for a dashed outline, or `bg-muted` for a filled one) containing `EmptyHeader` (`EmptyMedia variant="icon"` with a Phosphor icon, `EmptyTitle` "No teammates yet", `EmptyDescription` with a next step) and `EmptyContent` with a default `Button` for the CTA. |
| No search results | Empty with the query echoed in the description and a "Clear search" `secondary` button. |
| Load error | `Alert variant="destructive"` (Warning icon) with title "Couldn’t load members", a description, and an `AlertAction` holding a `Button size="sm" variant="secondary"` labelled "Retry". |
| Field error | §4.1. Summarise at the top with an Alert only on long forms. |

### 4.8 Page header and layout

```tsx
<SidebarProvider>
  <Sidebar collapsible="offcanvas">…</Sidebar>
  <SidebarInset>
    <div className="mx-auto flex w-full max-w-170 flex-col gap-3.5 px-5 py-8 md:px-9">
      <Breadcrumb>…</Breadcrumb>                                    {/* optional, nested pages */}
      <div className="flex items-center gap-2">
        <SidebarTrigger className="-ms-2" />
        <h1 className="text-3xl font-semibold tracking-tight">Members</h1>   {/* blocks use 3xl–4xl */}
        <Button className="ms-auto">Invite</Button>
      </div>
      <p className="text-muted-foreground">Manage who can access this workspace.</p>
      {/* sections: <h2 className="-mb-3 px-1 text-sm font-semibold">…</h2> + inset group, 20px apart */}
    </div>
  </SidebarInset>
</SidebarProvider>
```

- **Settings sub-navigation:** a sidebar inside the settings area with group labels (12 px semibold muted) and 36 px menu buttons. Mark the current page with `isActive`; that gives a 6 % fill and a blue icon.
- **Within a page:** use `Tabs` (default variant) for peer views. Use the `line` variant when the tabs sit directly under the page title.

---

## 5. Accessibility

- **Base UI handles behaviour:**
  - Overlays: focus trap, focus return to the trigger, `Escape` to close, portals.
  - Composite widgets (menu, select, radio, tabs, toggle group): roving tabindex and arrow keys.
  - `aria-expanded`, `aria-checked` and `aria-selected`.
- **Styling hooks:** state is exposed as data attributes (`data-open`, `data-closed`, `data-checked`, `data-unchecked`, `data-disabled`, `data-highlighted`, `data-active`, `data-popup-open`, `data-starting-style`, `data-ending-style`). `shadcn/tailwind.css` defines the `data-open:` and similar variants.
- **Names:**
  - Every control needs a label (`FieldLabel htmlFor` or a wrapping `Label`). Placeholders and addon text do not name a field.
  - Icon-only buttons need an `aria-label`; tooltips do not name their trigger.
  - Always include a `DialogTitle`, `AlertDialogTitle` and `SheetTitle`.
- **Semantics to add yourself:**
  - `CardTitle` and `EmptyTitle` are `div`s; put an `h2` or `h3` inside when they belong in the outline.
  - `Item`s inside an `ItemGroup` need `role="listitem"`.
  - Set `aria-invalid` on invalid controls.
  - Name radio groups and toggle groups with `aria-label` or `aria-labelledby`.
- **Announcements:** `FieldError` and `Alert` have `role="alert"`; the toast viewport is a "Notifications" region; Spinner is `role="status"`; add `aria-busy` to loading regions.
- **Dismissal:**
  - AlertDialog does not close on overlay click.
  - Dialog, Sheet and Popover close on `Escape` or an outside click.
  - The sidebar rail is out of the tab order, so keep a `SidebarTrigger`.
- **Target sizes:** all controls meet WCAG 2.2 AA's 24×24 px minimum. Checkbox, radio and switch extend their hit areas with `after:-inset-x-3 after:-inset-y-2`. For touch-heavy views prefer default or larger sizes (36–44 px); avoid `xs` (24 px).
- **Contrast** (computed from the tokens):

| Pair | Ratio | Verdict |
|---|---|---|
| `muted-foreground` on white | 5.1:1 | Pass |
| `muted-foreground` on `muted` | 4.7:1 | Pass |
| `primary` on white | 4.7:1 | Pass |
| `destructive` on white | 5.7:1 | Pass |
| **Dark-mode default button** (white on #2997ff) | **3.0:1** | AA only for large text. Flag in dark-mode reviews. |
| Filled inputs (4 % black) against a white page | 1.09:1 | Relies on labels and layout; WCAG 1.4.11 caveat |
| Unchecked switch track | 3.6:1 | Pass |

- **Shortcuts:** `Kbd` and `DropdownMenuShortcut` are hidden below `md`, so do not rely on them alone.
- **Motion:** suiss honours `prefers-reduced-motion` for indicators, toasts and the theme switch. Custom transitions should add `motion-reduce:transition-none`.

---

## 6. Do / don't and known gotchas

**Do**

- Use one `default` button per region; make the rest secondary, outline or ghost.
- Use `Switch` for instant settings, and `Checkbox` plus an explicit Save for forms.
- Group settings in inset lists with a `text-sm font-semibold` section heading above each.
- Use `Select size="sm"` or a `sm` segmented control as trailing controls in rows; use the full-height (44 px) controls in forms.
- Use the token classes (`bg-muted`, `text-muted-foreground`) and the Tailwind palette for status colours.
- Reach for the responsive patterns (`confirm`, `modal`, `panel`, `picker`, `action-menu`, `table-list`, `trail`, `app-shell`) when mobile matters.

**Don't**

- Don't add borders to inputs, use `font-medium`, or use 14 px medium labels. That is shadcn-default muscle memory, not suiss.
- Don't use solid red buttons, or coloured backgrounds on default badges, unless designed deliberately. If you do, document it as a Richie extension.
- Don't mix the two section-heading styles on one page: block style `text-sm font-semibold` versus the `FieldSet` inset legend, which is uppercase `text-xs` muted.
- Don't use `rounded-4xl`. Our `globals.css` defines it as 2.6× (smaller than 3xl); it is a shadcn-preset leftover that `theme-suiss` does not have.
- Don't put essential info in tooltips or toasts.

**Gotchas**

1. **Never hand-copy component source from registry JSON or docs.** It contains `@/components/...`, `@/lib/utils` and `IconPlaceholder`. The CLI rewrites those to `#components/ui/…`, `#lib/utils`, `#hooks/…` and Phosphor imports (seen in our installed `button.tsx`).
2. **Radix habits break.** No `asChild`. Use `render={<Link …/>}`, or apply `buttonVariants({ variant })` to a router `Link`. The MCP server returns Radix-base code.
3. **`SelectTrigger` is `w-fit`.** Add `className="w-full"` in forms.
4. **`AlertDialogAction` and `AlertDialogCancel` are close buttons.** Use a plain `Button` for async actions.
5. **`Empty` has `border-dashed` without a border width.** Add `border` to see the outline.
6. **`TooltipProvider` defaults to `delay={0}`,** so tooltips appear instantly.
7. **Sidebar internals bump `--radius` to 12 px,** so nested `rounded-*` values scale by 1.2.
8. **`cn-menu-target` and `cn-menu-translucent` are install-time marker classes.** The shadcn CLI handles them according to `menuColor` (`default` here) and `menuAccent` (`subtle`). With our settings, menus render opaque `bg-popover`. If you change `menuColor`, the CLI may need `--menu-*` tokens that our CSS lacks **(unverified)**.
9. **Mobile switch point is 48rem (768 px)** (`useIsMobile`). Patterns swap components there, the sidebar becomes a Sheet, and Kbd hides.
10. **`cn` is the `cn` npm package** (a clsx + tailwind-merge replacement), re-exported from `#lib/utils`.
11. **The toast API is `toast.add(...)` from Base UI's toast manager**, not sonner's `toast.success(...)`. Mount `<Toaster>` once.

---

## 7. For designers mocking up in static HTML

Reproduce suiss by **copying the tokens below and using the same `data-slot` / `data-variant` / `data-size` attributes the real components render**. That way implementers can map a mock 1:1 onto components.

**Setup:**

- **Font:** load Inter from Google Fonts (`family=Inter:wght@400;600;700`) and keep the stack `-apple-system, BlinkMacSystemFont, "Inter Variable", "Inter", sans-serif`.
- **Icons:** Phosphor regular. Either inline SVGs from phosphoricons.com, or the web font `https://unpkg.com/@phosphor-icons/web@2.1.1` with `<i class="ph ph-check"></i>` sized by `font-size` (that package version is unverified).
- **Dark mode:** put `class="dark"` on `<html>`.
- **Check at both widths:** mock at 1280 px and 390 px. Below 768 px, dialogs and sheets become bottom drawers and the sidebar becomes a sheet.

```css
/* ---------- suiss tokens (exact copy of globals.css values) ---------- */
:root{
  --radius:.625rem;
  --background:oklch(1 0 0);--foreground:oklch(.2316 .0038 286.09);
  --card:oklch(1 0 0);--card-foreground:oklch(.2316 .0038 286.09);
  --popover:oklch(1 0 0);--popover-foreground:oklch(.2316 .0038 286.09);
  --primary:oklch(.5629 .1933 256.16);--primary-foreground:oklch(1 0 0);
  --secondary:oklch(0 0 0/4%);--secondary-foreground:oklch(.2316 .0038 286.09);
  --muted:oklch(.9707 .0026 286.29);--muted-foreground:oklch(.5399 .0077 286.13);
  --accent:oklch(0 0 0/6%);--accent-foreground:oklch(.2316 .0038 286.09);
  --destructive:oklch(.5329 .2255 27.27);
  --border:oklch(.8652 .0068 286.24);--input:oklch(.6217 .0074 286.17);--ring:oklch(.5629 .1933 256.16);
  --sidebar:oklch(.9707 .0026 286.29);--sidebar-foreground:oklch(.2316 .0038 286.09);
  --sidebar-accent:oklch(0 0 0/6%);--sidebar-primary:oklch(.5629 .1933 256.16);--sidebar-border:oklch(.8652 .0068 286.24);
  --font-sans:-apple-system,BlinkMacSystemFont,"Inter Variable","Inter",sans-serif;
  --r-xs:calc(var(--radius)*.3);--r-sm:calc(var(--radius)*.5);--r-md:calc(var(--radius)*.8);--r-lg:var(--radius);
  --r-xl:calc(var(--radius)*1.2);--r-2xl:calc(var(--radius)*1.8);--r-3xl:calc(var(--radius)*2.8);
  --shadow-sm:0 1px 3px 0 rgb(0 0 0/.1),0 1px 2px -1px rgb(0 0 0/.1);
  --shadow-md:0 4px 6px -1px rgb(0 0 0/.1),0 2px 4px -2px rgb(0 0 0/.1);
  --shadow-lg:0 10px 15px -3px rgb(0 0 0/.1),0 4px 6px -4px rgb(0 0 0/.1);
  --shadow-xl:0 20px 25px -5px rgb(0 0 0/.1),0 8px 10px -6px rgb(0 0 0/.1);
  --hairline:0 0 0 1px color-mix(in oklab,var(--foreground) 5%,transparent);
  --focus:0 0 0 3px color-mix(in oklab,var(--ring) 50%,transparent);
  --ease-ios:cubic-bezier(.32,.72,0,1);
  --tinted-fg:color-mix(in oklab,var(--primary) 80%,var(--foreground));
}
.dark{
  --background:oklch(0 0 0);--foreground:oklch(.9707 .0026 286.29);
  --card:oklch(.2316 .0038 286.09);--card-foreground:oklch(.9707 .0026 286.29);
  --popover:oklch(.2316 .0038 286.09);--popover-foreground:oklch(.9707 .0026 286.29);
  --primary:oklch(.669 .1808 251.84);--primary-foreground:oklch(1 0 0);
  --secondary:oklch(1 0 0/10%);--secondary-foreground:oklch(.9707 .0026 286.29);
  --muted:oklch(.2006 .002 286.21);--muted-foreground:oklch(.7105 .0072 286.2);
  --accent:oklch(1 0 0/10%);--accent-foreground:oklch(.9707 .0026 286.29);
  --destructive:oklch(.7033 .2236 28.29);
  --border:oklch(.3802 .005 286.15);--input:oklch(.5399 .0077 286.13);--ring:oklch(.5629 .1933 256.16);
  --sidebar:oklch(.2006 .002 286.21);--sidebar-foreground:oklch(.9707 .0026 286.29);
  --sidebar-accent:oklch(1 0 0/10%);--sidebar-primary:oklch(.669 .1808 251.84);--sidebar-border:oklch(.3802 .005 286.15);
  --hairline:0 0 0 1px color-mix(in oklab,var(--foreground) 10%,transparent);
  --tinted-fg:color-mix(in oklab,var(--primary) 80%,var(--foreground));
}
*,*::before,*::after{box-sizing:border-box;border-color:var(--border)}
body{margin:0;background:var(--background);color:var(--foreground);font:400 16px/1.5 var(--font-sans)}
.ph,[data-slot] svg{flex-shrink:0;font-size:16px;width:1em;height:1em}

/* ---------- Button ---------- */
[data-slot=button]{display:inline-flex;align-items:center;justify-content:center;gap:6px;height:36px;padding:0 16px;
  border:1px solid transparent;border-radius:9999px;background-clip:padding-box;font:400 14px/20px var(--font-sans);
  white-space:nowrap;cursor:pointer;outline:none;transition:color .15s,background-color .15s,border-color .15s;
  background-color:var(--primary);color:var(--primary-foreground)}
[data-slot=button][data-variant=default]:hover{background-color:color-mix(in oklab,var(--primary) 80%,transparent)}
[data-slot=button][data-variant=secondary]{background-color:var(--secondary);color:var(--secondary-foreground)}
[data-slot=button][data-variant=secondary]:hover{background-color:color-mix(in oklab,var(--secondary) 80%,transparent)}
[data-slot=button][data-variant=outline]{background-color:var(--background);border-color:var(--border);color:var(--foreground)}
.dark [data-slot=button][data-variant=outline]{background-color:transparent}
[data-slot=button][data-variant=tinted]{background-color:color-mix(in oklab,var(--primary) 10%,transparent);color:var(--tinted-fg)}
.dark [data-slot=button][data-variant=tinted]{background-color:color-mix(in oklab,var(--primary) 20%,transparent)}
[data-slot=button][data-variant=ghost],[data-slot=button][data-variant=plain],[data-slot=button][data-variant=link]{background-color:transparent;color:var(--foreground)}
[data-slot=button][data-variant=plain],[data-slot=button][data-variant=link]{color:var(--primary)}
[data-slot=button]:is([data-variant=outline],[data-variant=ghost],[data-variant=plain]):hover{background-color:var(--accent)}
[data-slot=button][data-variant=link]:hover{text-decoration:underline}
[data-slot=button][data-variant=destructive]{background-color:color-mix(in oklab,var(--destructive) 10%,transparent);color:var(--destructive)}
[data-slot=button][data-variant=destructive]:hover{background-color:color-mix(in oklab,var(--destructive) 20%,transparent)}
.dark [data-slot=button][data-variant=destructive]{background-color:color-mix(in oklab,var(--destructive) 20%,transparent)}
[data-slot=button]:focus-visible{border-color:var(--ring);box-shadow:var(--focus)}
[data-slot=button][data-variant=destructive]:focus-visible{border-color:color-mix(in oklab,var(--destructive) 40%,transparent);box-shadow:0 0 0 3px color-mix(in oklab,var(--destructive) 20%,transparent)}
[data-slot=button]:disabled{opacity:.5;pointer-events:none}
[data-slot=button][data-size=xs]{height:24px;padding:0 11px;gap:4px;font-size:12px;line-height:16px}
[data-slot=button][data-size=xs] :is(svg,.ph){font-size:12px}
[data-slot=button][data-size=sm]{height:32px;padding:0 12px;gap:4px}
[data-slot=button][data-size=lg]{height:44px;padding:0 22px;font-size:16px;line-height:24px}
[data-slot=button][data-size=xl]{height:56px;padding:0 31px;gap:8px;font-size:16px;line-height:24px}
[data-slot=button][data-size^=icon]{padding:0;width:36px;height:36px}
[data-slot=button][data-size=icon-xs]{width:24px;height:24px}[data-slot=button][data-size=icon-sm]{width:32px;height:32px}
[data-slot=button][data-size=icon-lg]{width:44px;height:44px}[data-slot=button][data-size=icon-xl]{width:56px;height:56px}
[data-slot=button][data-block]{width:100%;border-radius:var(--r-md)} /* xs: --r-sm, lg: --r-lg, xl: --r-xl */

/* ---------- Fields ---------- */
[data-slot=input],[data-slot=select-trigger],[data-slot=native-select],[data-slot=textarea]{width:100%;height:44px;padding:4px 12px;
  border:1px solid transparent;border-radius:var(--r-lg);background:var(--secondary);color:var(--foreground);
  font:400 16px/24px var(--font-sans);caret-color:var(--primary);outline:none;transition:color .15s,box-shadow .15s,background-color .15s}
[data-slot=select-trigger]{display:flex;align-items:center;justify-content:space-between;gap:6px;width:fit-content;padding:8px 12px}
[data-slot=select-trigger] .ph{color:var(--muted-foreground)}
[data-slot=textarea]{height:auto;min-height:64px;padding:12px;border-radius:var(--r-xl);resize:none}
:is([data-slot=input],[data-slot=textarea])::placeholder{color:var(--muted-foreground)}
[data-slot=select-trigger][data-placeholder]{color:var(--muted-foreground)}
:is([data-slot=input],[data-slot=select-trigger],[data-slot=native-select],[data-slot=textarea]):focus-visible{border-color:var(--ring);box-shadow:var(--focus)}
[aria-invalid=true]:is([data-slot=input],[data-slot=select-trigger],[data-slot=native-select],[data-slot=textarea]){border-color:var(--destructive);
  box-shadow:0 0 0 3px color-mix(in oklab,var(--destructive) 20%,transparent);background:color-mix(in oklab,var(--destructive) 5%,transparent)}
[data-slot=input][data-size=md]{height:36px;font-size:14px;line-height:20px}
[data-slot=input][data-size=sm]{height:32px;padding-inline:10px;border-radius:var(--r-md);font-size:14px;line-height:20px}
[data-slot=select-trigger][data-size=sm]{height:36px;border-radius:var(--r-md)}
:is([data-slot=input],[data-slot=textarea]):disabled{opacity:.5;cursor:not-allowed}
[data-slot=label],[data-slot=field-label]{display:flex;align-items:center;gap:8px;font-size:16px;line-height:24px;font-weight:600}
[data-slot=field-group]{display:flex;flex-direction:column;gap:28px}
[data-slot=field]{display:flex;flex-direction:column;gap:12px;width:100%}
[data-slot=field][data-orientation=horizontal]{flex-direction:row;align-items:center}
[data-slot=field-content]{display:flex;flex:1;flex-direction:column;gap:4px}
[data-slot=field-description]{margin:0;font-size:14px;line-height:20px;color:var(--muted-foreground)}
[data-slot=field-error]{font-size:14px;line-height:20px;color:var(--destructive)}

/* ---------- Switch / Checkbox / Radio ---------- */
[data-slot=switch]{position:relative;display:inline-flex;align-items:center;flex-shrink:0;width:52px;height:24px;border:2px solid transparent;
  border-radius:9999px;background:var(--input);cursor:pointer;transition:background-color .15s,border-color .15s}
.dark [data-slot=switch]{background:color-mix(in oklab,var(--input) 80%,transparent)}
[data-slot=switch][data-checked]{background:var(--primary);border-color:var(--primary)}
[data-slot=switch-thumb]{display:block;width:32px;height:20px;border-radius:9999px;background:var(--background);box-shadow:var(--shadow-sm);
  transition:translate .2s var(--ease-ios),width .2s var(--ease-ios)}
.dark [data-slot=switch-thumb]{background:var(--foreground)}
[data-slot=switch][data-checked] [data-slot=switch-thumb]{translate:16px 0}
[data-slot=switch][data-size=sm]{width:44px;height:20px}
[data-slot=switch][data-size=sm] [data-slot=switch-thumb]{width:26px;height:16px}
[data-slot=switch][data-size=sm][data-checked] [data-slot=switch-thumb]{translate:14px 0}
[data-slot=checkbox],[data-slot=radio-group-item]{display:inline-flex;align-items:center;justify-content:center;flex-shrink:0;width:16px;height:16px;
  border:1px solid color-mix(in oklab,var(--foreground) 25%,transparent);border-radius:calc(var(--radius)*.4);background:var(--background);
  box-shadow:0 .5px 1px color-mix(in oklab,var(--foreground) 12%,transparent);color:var(--primary-foreground)}
[data-slot=radio-group-item]{border-radius:9999px}
:is([data-slot=checkbox],[data-slot=radio-group-item])[data-checked]{background:var(--primary);border-color:var(--primary);box-shadow:none}
[data-slot=checkbox] .ph{font-size:14px}
[data-slot=radio-group-item][data-checked]::after{content:"";width:8px;height:8px;border-radius:50%;background:var(--primary-foreground)}
.dark :is([data-slot=checkbox],[data-slot=radio-group-item]):not([data-checked]){border-color:transparent;background:var(--border);box-shadow:none}

/* ---------- Surfaces ---------- */
[data-slot=card]{display:flex;flex-direction:column;gap:24px;padding:24px 0;border-radius:var(--r-2xl);background:var(--card);color:var(--card-foreground);
  box-shadow:var(--hairline),var(--shadow-md);overflow:hidden}
[data-slot=card]>*{padding-inline:24px}
[data-slot=card][data-size=sm]{gap:16px;padding-block:16px}[data-slot=card][data-size=sm]>*{padding-inline:16px}
[data-slot=card][data-variant=filled]{background:var(--muted);box-shadow:none}
[data-slot=card-title],[data-slot=dialog-title],[data-slot=sheet-title]{font-size:16px;line-height:24px;font-weight:600}
[data-slot=card-description],[data-slot=dialog-description],[data-slot=sheet-description]{font-size:14px;line-height:20px;color:var(--muted-foreground)}
/* inset grouped list (ItemGroup / FieldGroup variant=inset) */
[data-variant=inset]:is([data-slot=item-group],[data-slot=field-group]){display:flex;flex-direction:column;gap:0;border-radius:var(--r-2xl);background:var(--muted);overflow:hidden}
[data-slot=item-separator]{height:1px;background:var(--border);margin-inline-start:16px}
[data-slot=item]{display:flex;flex-wrap:wrap;align-items:center;gap:14px;padding:14px 16px;border:1px solid transparent;border-radius:var(--r-xl);font-size:16px;line-height:24px}
[data-slot=item][data-size=sm]{padding:12px 14px}[data-slot=item][data-size=xs]{gap:10px;padding:10px 12px}
[data-slot=item][data-variant=outline]{border-color:var(--border)}
[data-slot=item][data-variant=muted]{background:color-mix(in oklab,var(--muted) 50%,transparent)}
[data-variant=inset] [data-slot=item]{border-radius:0}
a[data-slot=item]:hover{background:var(--accent)}
[data-slot=item-content]{display:flex;flex:1;flex-direction:column;gap:4px;min-width:0}
[data-slot=item-title]{font-weight:600;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
[data-variant=inset] [data-slot=item-title]{font-weight:400} /* block convention: code must pass className="text-base font-normal"; not automatic */
[data-slot=item-description]{margin:0;font-size:14px;line-height:20px;color:var(--muted-foreground)}
[data-slot=item-actions]{display:flex;align-items:center;gap:8px}
[data-variant=inset][data-slot=field-group]>[data-slot=field]{position:relative;min-height:44px;padding:6px 16px}
[data-variant=inset][data-slot=field-group]>[data-slot=field]:not(:first-child)::before{content:"";position:absolute;top:0;inset-inline:16px 0;height:1px;background:var(--border)}
[data-variant=inset][data-slot=field-group] [data-slot=field-label]{font-weight:400}
[data-variant=inset][data-slot=field-group] [data-slot=input]{height:32px;padding-inline:0;background:transparent;text-align:end;box-shadow:none}
.section-title{margin:0 0 -12px;padding:0 4px;font-size:14px;line-height:20px;font-weight:600} /* block-style heading above an inset group */
.value-card{display:flex;align-items:center;gap:12px;padding:18px 20px;border-radius:var(--r-2xl);background:var(--muted)} /* settings-account row */
[data-slot=alert]{display:grid;grid-template-columns:auto 1fr;column-gap:12px;row-gap:2px;padding:20px;border-radius:var(--r-2xl);background:var(--muted)}
[data-slot=alert]>.ph{font-size:20px;color:var(--primary);grid-row:span 2;translate:0 1px}
[data-slot=alert][data-variant=destructive]>.ph{color:var(--destructive)}
[data-slot=alert-title]{font-weight:600}[data-slot=alert-description]{color:var(--muted-foreground)}
[data-slot=empty]{display:flex;flex-direction:column;align-items:center;justify-content:center;gap:16px;padding:48px;border-radius:var(--r-2xl);text-align:center}
[data-slot=empty-icon]{display:flex;align-items:center;justify-content:center;width:40px;height:40px;margin-bottom:8px;border-radius:var(--r-lg);background:var(--muted)}
[data-slot=empty-icon] .ph{font-size:20px}
[data-slot=empty-title]{font-size:18px;line-height:28px;font-weight:600}
[data-slot=empty-description]{max-width:384px;font-size:14px;line-height:1.625;color:var(--muted-foreground)}
[data-slot=skeleton]{border-radius:var(--r-md);background:var(--muted);animation:pulse 2s cubic-bezier(.4,0,.6,1) infinite}
@keyframes pulse{50%{opacity:.5}}

/* ---------- Badge / Avatar / Kbd / Table ---------- */
[data-slot=badge]{display:inline-flex;align-items:center;gap:4px;height:20px;border:1px solid transparent;border-radius:9999px;font-size:12px;line-height:16px;font-weight:600;color:var(--primary)}
[data-slot=badge][data-variant=secondary]{color:var(--muted-foreground)}[data-slot=badge][data-variant=destructive]{color:var(--destructive)}
[data-slot=badge][data-variant=outline]{padding:0 8px;border-color:var(--border);color:var(--foreground)}
[data-slot=badge][data-variant=ghost]{padding:0 8px;color:inherit}
[data-slot=avatar]{position:relative;display:flex;flex-shrink:0;width:32px;height:32px;border-radius:9999px}
[data-slot=avatar][data-size=sm]{width:24px;height:24px}[data-slot=avatar][data-size=lg]{width:40px;height:40px}[data-slot=avatar][data-size=xl]{width:64px;height:64px}
[data-slot=avatar] img{width:100%;height:100%;border-radius:9999px;object-fit:cover}
[data-slot=avatar-fallback]{display:flex;align-items:center;justify-content:center;width:100%;height:100%;border-radius:9999px;
  background:linear-gradient(to bottom,color-mix(in oklab,var(--muted-foreground) 90%,transparent),var(--muted-foreground));
  color:var(--primary-foreground);font-size:12px;font-weight:600;text-transform:uppercase}
.dark [data-slot=avatar-fallback]{background:linear-gradient(to bottom,color-mix(in oklab,var(--muted-foreground) 70%,transparent),color-mix(in oklab,var(--muted-foreground) 50%,transparent))}
[data-slot=avatar][data-size=lg] [data-slot=avatar-fallback]{font-size:16px}[data-slot=avatar][data-size=xl] [data-slot=avatar-fallback]{font-size:24px}
[data-slot=kbd]{display:inline-flex;align-items:center;justify-content:center;gap:4px;height:22px;min-width:22px;padding:0 6px;border-radius:var(--r-sm);background:var(--muted);color:var(--muted-foreground);font:400 12px/16px var(--font-sans)}
[data-slot=table]{width:100%;border-collapse:collapse;font-size:14px;line-height:20px}
[data-slot=table-head]{height:48px;padding:0 12px;text-align:start;font-size:12px;font-weight:600;color:var(--muted-foreground);white-space:nowrap}
[data-slot=table-cell]{padding:12px;white-space:nowrap;font-variant-numeric:tabular-nums}
[data-slot=table-row]{border-bottom:1px solid var(--border)}[data-slot=table-body] [data-slot=table-row]:last-child{border-bottom:0}
[data-slot=table-body] [data-slot=table-row]:hover{background:var(--accent)}

/* ---------- Tabs / Segmented / Tooltip ---------- */
[data-slot=tabs-list-content],[data-slot=toggle-group][data-spacing="0"]{display:inline-flex;align-items:center;gap:4px;padding:4px;border-radius:var(--r-xl);background:var(--secondary)}
[data-slot=tabs-list-content]{height:36px;color:var(--muted-foreground)}
[data-slot=tabs-trigger]{display:inline-flex;flex:1;align-items:center;justify-content:center;gap:8px;height:100%;padding:4px 12px;border:0;border-radius:var(--r-md);background:none;font:400 14px/20px var(--font-sans);color:var(--muted-foreground)}
[data-slot=tabs-trigger]:hover{color:var(--foreground)}
[data-slot=toggle-group-item]{display:inline-flex;align-items:center;justify-content:center;gap:4px;height:36px;min-width:36px;padding:0 12px;border:0;border-radius:var(--r-md);background:none;font:400 14px/20px var(--font-sans);color:inherit}
[data-slot=toggle-group-item][data-size=sm]{height:32px;min-width:32px}
[data-slot=tabs-trigger][data-active],[data-slot=toggle-group-item][aria-pressed=true]{background:var(--card);color:var(--card-foreground);box-shadow:var(--shadow-sm)}
.dark :is([data-slot=tabs-trigger][data-active],[data-slot=toggle-group-item][aria-pressed=true]){background:var(--border)}
[data-slot=tooltip-content]{display:inline-flex;align-items:center;gap:6px;max-width:320px;padding:6px 12px;border-radius:var(--r-xl);background:var(--foreground);color:var(--background);font-size:12px;line-height:16px}

/* ---------- Menus / Select popup ---------- */
[data-slot=dropdown-menu-content],[data-slot=select-content],[data-slot=combobox-content]{min-width:192px;padding:6px;border-radius:var(--r-xl);
  background:var(--popover);color:var(--popover-foreground);box-shadow:var(--hairline),var(--shadow-lg)}
[data-slot=select-content]{min-width:144px}
:is([data-slot=dropdown-menu-item],[data-slot=select-item],[data-slot=combobox-item]){position:relative;display:flex;align-items:center;gap:10px;
  padding:8px 12px;border-radius:var(--r-md);font-size:16px;line-height:24px;cursor:default}
:is([data-slot=select-item],[data-slot=combobox-item],[data-slot=dropdown-menu-checkbox-item]){padding-inline-end:32px}
:is([data-slot=dropdown-menu-item],[data-slot=select-item],[data-slot=combobox-item]):is(:hover,[data-highlighted]){background:var(--accent)}
[data-slot=select-item] .check{position:absolute;inset-inline-end:8px;color:var(--primary)}
[data-slot=dropdown-menu-item][data-variant=destructive]{color:var(--destructive)}
[data-slot=dropdown-menu-item][data-variant=destructive]:hover{background:color-mix(in oklab,var(--destructive) 10%,transparent)}
:is([data-slot=dropdown-menu-label],[data-slot=select-label]){padding:10px 12px;font-size:12px;line-height:16px;font-weight:600;color:var(--muted-foreground)}
[data-slot=dropdown-menu-separator]{height:1px;margin:6px -6px;background:color-mix(in oklab,var(--border) 50%,transparent)}
[data-slot=dropdown-menu-shortcut]{margin-inline-start:auto;font-size:12px;color:var(--muted-foreground)}

/* ---------- Overlays ---------- */
[data-slot$=-overlay]{position:fixed;inset:0;z-index:50;background:rgb(0 0 0/.5)}
[data-slot=dialog-content],[data-slot=alert-dialog-content]{position:fixed;top:50%;left:50%;z-index:50;translate:-50% -50%;display:grid;gap:24px;
  width:100%;max-width:min(calc(100% - 2rem),28rem);padding:24px;border-radius:var(--r-2xl);background:var(--popover);color:var(--popover-foreground);
  box-shadow:var(--hairline),var(--shadow-xl)}
[data-slot=alert-dialog-content]{max-width:min(calc(100% - 2rem),21.25rem)}
[data-slot=alert-dialog-content][data-size=sm]{max-width:min(calc(100% - 2rem),20rem)}
[data-slot=dialog-header],[data-slot=sheet-header]{display:flex;flex-direction:column;gap:6px}
[data-slot=dialog-footer]{display:flex;justify-content:flex-end;gap:8px}
@media (max-width:639px){[data-slot=dialog-footer]{flex-direction:column-reverse}}
[data-slot=dialog-close]{position:absolute;top:19px;inset-inline-end:16px} /* = button ghost icon-sm with --secondary fill */
[data-slot=alert-dialog-header]{display:grid;place-items:center;gap:6px;text-align:center}
[data-slot=alert-dialog-title]{font-size:18px;line-height:28px;font-weight:600}
[data-slot=alert-dialog-description]{font-size:14px;line-height:20px;color:var(--muted-foreground);text-wrap:balance}
[data-slot=alert-dialog-footer]{display:grid;grid-template-columns:1fr 1fr;gap:8px}
[data-slot=alert-dialog-media]{display:inline-flex;align-items:center;justify-content:center;width:64px;height:64px;margin-bottom:8px;border-radius:9999px;background:var(--muted)}
[data-slot=alert-dialog-media] .ph{font-size:32px}
[data-slot=sheet-content]{position:fixed;top:8px;bottom:8px;right:8px;z-index:50;display:flex;flex-direction:column;width:75%;max-width:24rem;
  border-radius:var(--r-3xl);background:var(--popover);color:var(--popover-foreground);box-shadow:var(--shadow-xl)}
[data-slot=sheet-header]{padding:24px}[data-slot=sheet-footer]{margin-top:auto;padding:24px;display:flex;flex-direction:column;gap:8px}
[data-slot=popover-content]{display:flex;flex-direction:column;gap:16px;width:288px;padding:16px;border-radius:var(--r-2xl);background:var(--popover);box-shadow:var(--hairline),var(--shadow-lg)}
[data-slot=toast-viewport]{position:fixed;right:16px;bottom:16px;z-index:50;width:100%;max-width:24rem}
[data-slot=toast]{border-radius:var(--r-xl);background:var(--popover);color:var(--popover-foreground);box-shadow:var(--hairline),0 8px 32px rgb(0 0 0/.14)}
.dark [data-slot=toast]{box-shadow:var(--shadow-lg)}
[data-slot=toast-content]{display:flex;align-items:center;gap:12px;padding:16px}
[data-slot=toast-icon] .ph{font-size:24px}
[data-slot=toast-title]{font-size:14px;line-height:20px;font-weight:600}[data-slot=toast-description]{font-size:14px;line-height:20px;color:var(--muted-foreground)}

/* ---------- Sidebar / Breadcrumb / Page ---------- */
[data-slot=sidebar-container]{display:flex;flex-direction:column;width:256px;height:100svh;background:var(--sidebar);color:var(--sidebar-foreground);border-inline-end:1px solid var(--sidebar-border)}
[data-slot=sidebar-group]{padding:8px}
[data-slot=sidebar-group-label]{display:flex;align-items:center;height:32px;padding:0 12px;font-size:12px;font-weight:600;color:var(--muted-foreground)}
[data-slot=sidebar-menu]{display:flex;flex-direction:column;gap:2px;margin:0;padding:0;list-style:none}
[data-slot=sidebar-menu-button]{display:flex;align-items:center;gap:8px;width:100%;height:36px;padding:8px 12px;border:0;border-radius:.6rem;background:none;
  font:400 16px/24px var(--font-sans);color:inherit;text-align:start}
[data-slot=sidebar-menu-button]:is(:hover,[data-active]){background:var(--sidebar-accent)}
[data-slot=sidebar-menu-button][data-active] .ph{color:var(--sidebar-primary)}
[data-slot=breadcrumb-list]{display:flex;flex-wrap:wrap;align-items:center;gap:12px;margin:0;padding:0;list-style:none;font-size:14px;color:var(--muted-foreground)}
[data-slot=breadcrumb-page]{color:var(--foreground)}[data-slot=breadcrumb-separator] .ph{font-size:12px}
.page{display:flex;flex-direction:column;gap:14px;width:100%;max-width:42.5rem;margin:0 auto;padding:32px 20px}
@media (min-width:768px){.page{padding-inline:36px}}
.page h1{margin:0;font-size:30px;line-height:36px;font-weight:600;letter-spacing:-.025em} /* 36/40 for a hero page title */
```

**Mock checklist:**

- [ ] Inputs are filled, 44 px, with no visible border.
- [ ] Labels are 16 px / 600; buttons are 14 px / 400 pills.
- [ ] Only one blue default button per region; destructive buttons are tinted.
- [ ] Cards, dialogs and grouped lists use 18 px radius; menus 12 px; fields 10 px.
- [ ] Icons are Phosphor regular at 16 px.
- [ ] Dark mode checked, with true-black canvas and #1d1d1f cards.
- [ ] Focus state shown on at least one control (blue border plus a 3 px 50 % ring).

---

## 8. For implementers later

- **Install from `frontend/`:** `pnpm ui:add @esuiss/<name> [@esuiss/<name2> …]`. It runs `pnpm --filter @repo/ui exec shadcn add` inside `packages/ui`, using `packages/ui/components.json` (style `base-nova`, which selects the Base UI build; registry `@esuiss` → `https://suiss.app/ui/r/base-suiss/{name}.json`; icon library phosphor; aliases `#components`, `#components/ui`, `#lib`, `#hooks`).
- **Never run shadcn from an app** (`apps/*`). It writes `@esuiss/utils` and the components into the app.
- **Install locations:**
  - suiss items nest under `packages/ui/src/components/ui/`; hooks go to `src/hooks/`; `cn` already lives in `src/lib/utils.ts`.
  - The docs say patterns go to `components/patterns/` and blocks to `components/blocks/`. Confirm the path the CLI prints before importing **(unverified in this repo)**.
- **Imports in apps:** `import { Button } from "@repo/ui/components/ui/button"`. Import the stylesheet once in the root route: `import "@repo/ui/globals.css"` (as `apps/identity/src/routes/__root.tsx` already does). Inside `packages/ui`, components import each other via `#components/ui/<x>`, `#lib/utils` and `#hooks/<x>`.
- **Theme:** do **not** add `@esuiss/theme-suiss`; our tokens are already identical. Add `@esuiss/theme` only if a light/dark switch is needed. It appends keyframes to the stylesheet; for Vite, put the no-flash script in `index.html`.
- **Dependencies the CLI will pull:**
  - `@phosphor-icons/react` with the first icon-bearing component (from the CLI's icon-library map; exact behaviour unverified).
  - `input-otp` (input-otp) and `cmdk` (command).
  - Registry deps such as `use-scrollbar-gap` (dialog, sheet, alert-dialog), `use-platform` (kbd, menus) and `use-mobile` (sidebar, patterns).
- **App-level providers:** mount `<Toaster>` once, `SidebarProvider` around the shell, and optionally `TooltipProvider`.
- **Composition (Base UI):**
  - Use `render` to swap elements: `<DialogTrigger render={<Button variant="secondary" />}>`.
  - For TanStack Router links styled as buttons, prefer `<Link className={buttonVariants({ variant: "secondary" })}>` over `<Button render={<Link/>}>`. Base UI's Button may need `nativeButton={false}` for non-button elements **(unverified)**.
  - Style state through data attributes, never by re-implementing state classes.
- **Customising:** edit the installed file in `packages/ui`, since you own the code. Re-adding with `--overwrite` replaces local edits; `add` warns when a file differs from the registry.
- **Reference code:**
  - Fetch examples as registry items (`curl -s https://suiss.app/ui/r/base-suiss/<example>.json | jq -r '.files[0].content'`) to see intended usage. Rewrite the `@/` imports and `IconPlaceholder` by hand only in throwaway mocks.
  - Blocks: see `suiss-blocks-catalog.md`.

---

## 9. Unverified or open

| Item | Status |
|---|---|
| Exact on-disk path for `registry:component` patterns and blocks in this repo (`components/patterns/` per docs vs nested under `components/ui/`) | Not run (no installs allowed) |
| How the CLI rewrites `cn-menu-target` / `cn-menu-translucent` for `menuColor: default` | Transformer seen in shadcn 4.23 dist; output not traced |
| `@phosphor-icons/react` auto-added as a dependency, and how Phosphor glyphs render under the toast `fill-*` classes (a lucide-style filled circle with a white glyph vs a coloured outline) | Not run or rendered |
| Toast default timeout (5000 ms is the Base UI default) | Not documented by suiss |
| `nativeButton={false}` needed when `Button` renders a link | Base UI API recollection |
| `@phosphor-icons/web@2.1.1` CDN version for mocks | Not fetched |
| 8-slot OTP fit on 375 px screens at the default slot size | Calculated (about 430 px wide), not rendered |
| Visual rendering of the suiss.app previews | Classes read from source; previews were not screenshotted |

---

## Appendix A: look-defining classes (verbatim, trimmed of behaviour-only utilities)

```txt
button        base: rounded-full border border-transparent bg-clip-padding text-sm whitespace-nowrap transition-colors focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/50 disabled:opacity-50 aria-invalid:border-destructive aria-invalid:ring-3 aria-invalid:ring-destructive/20 [&_svg:not([class*='size-'])]:size-4
              default: bg-primary text-primary-foreground hover:bg-primary/80
              outline: border-border bg-background hover:bg-accent dark:bg-transparent
              secondary: bg-secondary text-secondary-foreground hover:bg-secondary/80
              tinted: bg-primary/10 text-[color-mix(in_oklab,var(--primary)_80%,var(--foreground))] hover:bg-primary/20 dark:bg-primary/20 dark:hover:bg-primary/30
              ghost: hover:bg-accent | plain: text-primary hover:bg-accent | link: text-primary hover:underline
              destructive: bg-destructive/10 text-destructive hover:bg-destructive/20 dark:bg-destructive/20 dark:hover:bg-destructive/30
              sizes: xs h-6 px-2.75 text-xs | sm h-8 px-3 | default h-9 gap-1.5 px-4 | lg h-11 px-5.5 text-base | xl h-14 px-7.75 text-base | icon size-9 (xs 6, sm 8, lg 11, xl 14)
              block radius: xs rounded-sm | sm/default rounded-md | lg rounded-lg | xl rounded-xl
input         h-11 rounded-lg border border-transparent bg-secondary px-3 py-1 text-base caret-primary placeholder:text-muted-foreground aria-invalid:bg-destructive/5 data-[size=md]:h-9 data-[size=md]:text-sm data-[size=sm]:h-8 data-[size=sm]:rounded-md data-[size=sm]:px-2.5 data-[size=sm]:text-sm
textarea      field-sizing-content min-h-16 rounded-xl border border-transparent bg-secondary px-3 py-3 text-base caret-primary
select-trigger w-fit gap-1.5 rounded-lg border border-transparent bg-secondary px-3 py-2 text-base data-placeholder:text-muted-foreground data-[size=default]:h-11 data-[size=sm]:h-9 data-[size=sm]:rounded-md
select-content min-w-36 rounded-xl bg-popover text-popover-foreground shadow-lg ring-1 ring-foreground/5 dark:ring-foreground/10
select-item   gap-2.5 rounded-md py-2 pe-8 ps-3 text-base   (highlight: absolute rounded-md bg-accent; check: text-primary)
label         gap-2 text-base font-semibold (font-normal when containing checkbox/radio/switch)
field         gap-3 (vertical) | field-group gap-7 | field-description text-sm text-muted-foreground | field-error text-sm text-destructive
field-group inset  rounded-2xl bg-muted; fields min-h-11 px-4 py-1.5; separators before:inset-s-4 before:h-px before:bg-border; labels font-normal; inputs h-8 bg-transparent px-0 text-end
choice card   (FieldLabel > Field) rounded-xl border p-4 has-data-checked:border-primary has-data-checked:ring-1 has-data-checked:ring-primary hover:bg-accent
switch        rounded-full border-2 h-6 w-13 (sm h-5 w-11) data-checked:bg-primary data-checked:border-primary data-unchecked:bg-input dark:data-unchecked:bg-input/80
switch-thumb  rounded-full bg-background shadow-sm h-5 w-8 (sm h-4 w-6.5) data-checked:translate-x-4 (sm 3.5) dark:bg-foreground  [duration-200 ease-[cubic-bezier(0.32,0.72,0,1)]]
checkbox      size-4 rounded-[calc(var(--radius)*0.4)] border border-foreground/25 bg-background shadow-[0_0.5px_1px_color-mix(in_oklab,var(--foreground)_12%,transparent)] data-checked:bg-primary data-checked:border-primary dark:not-data-checked:bg-border dark:not-data-checked:border-transparent; indicator svg size-3.5
radio         size-4 rounded-full (same as checkbox); dot size-2 (dark size-2.5) bg-primary-foreground
input-otp-slot h-13 w-11 rounded-lg border-[1.5px] border-transparent bg-secondary text-xl font-semibold tabular-nums data-[active=true]:border-primary; group gap-2
input-group   h-11 rounded-lg border border-transparent bg-secondary (ring on group when control focused); addon text-base text-muted-foreground ps-3/pe-3
combobox-chip h-[calc(--spacing(5.5))] rounded-full bg-primary/10 dark:bg-primary/20 px-2.5 text-xs text-[color-mix(in_oklab,var(--primary)_80%,var(--foreground))]
card          gap-6 py-6 [px-6 per part] rounded-2xl bg-card text-base shadow-md ring-1 ring-foreground/5 dark:ring-foreground/10; sm --card-spacing 4; filled bg-muted shadow-none ring-0; title text-base font-semibold; description text-sm text-muted-foreground
item          rounded-xl border text-base transition-colors; default border-transparent; outline border-border; muted bg-muted/50; sizes default gap-3.5 px-4 py-3.5 | sm gap-3.5 px-3.5 py-3 | xs gap-2.5 px-3 py-2.5
item parts    title gap-2 text-base font-semibold line-clamp-1 | description text-sm text-muted-foreground line-clamp-2 | content gap-1 | actions gap-2 | media image size-10 rounded-md
item-group    gap-4; inset: gap-0 overflow-hidden rounded-2xl bg-muted, items rounded-none, separators my-0 ms-4 (separator h-px bg-border)
badge         h-5 gap-1 rounded-full border border-transparent text-xs font-semibold [&>svg]:size-3!; default text-primary | secondary text-muted-foreground | destructive text-destructive | outline border-border px-2 text-foreground | ghost px-2
avatar        size-8 rounded-full (sm 6, lg 10, xl 16); fallback bg-linear-to-b from-muted-foreground/90 to-muted-foreground text-xs font-semibold text-primary-foreground uppercase (lg text-base, xl text-2xl) dark:from-muted-foreground/70 dark:to-muted-foreground/50
table         text-sm; head h-12 px-3 text-xs font-semibold text-muted-foreground; cell p-3 tabular-nums; row border-b hover:bg-accent data-selected:bg-accent
dialog        max-w-[calc(100%-2rem)] sm:max-w-md gap-6 rounded-2xl bg-popover p-6 text-base shadow-xl ring-1 ring-foreground/5; overlay bg-black/50; footer flex-col-reverse gap-2 sm:flex-row sm:justify-end; close absolute top-4.75 inset-e-4 (ghost icon-sm bg-secondary)
alert-dialog  max-w-xs data-[size=default]:max-w-85 gap-6 rounded-2xl bg-popover p-6 shadow-xl ring-1; header place-items-center gap-1.5 text-center; title text-lg font-semibold; footer grid grid-cols-2 gap-2; media size-16 rounded-full bg-muted *:svg:size-8
sheet         rounded-3xl bg-popover shadow-xl duration-400 ease-[cubic-bezier(0.32,0.72,0,1)]; right: inset-y-2 right-2 w-3/4 sm:max-w-sm; header/footer p-6 gap-1.5/gap-2
popover       w-72 gap-4 rounded-2xl bg-popover p-4 shadow-lg ring-1 ring-foreground/5
dropdown-menu content min-w-48 rounded-xl bg-popover p-1.5 shadow-lg ring-1 ring-foreground/5; item gap-2.5 rounded-md px-3 py-2 text-base focus:bg-accent; destructive text-destructive focus:bg-destructive/10; label px-3 py-2.5 text-xs font-semibold text-muted-foreground; separator -mx-1.5 my-1.5 h-px bg-border/50; shortcut ms-auto text-xs text-muted-foreground
tooltip       rounded-xl px-3 py-1.5 text-xs bg-foreground text-background max-w-xs; arrow size-2.5 rotate-45 rounded-xs bg-foreground
toast         rounded-xl bg-popover shadow-[0_8px_32px_rgb(0_0_0/0.14)] ring-1 ring-foreground/5 dark:shadow-lg dark:ring-0; content gap-3 p-4; title text-sm font-semibold; description text-sm text-muted-foreground; viewport bottom-4 inset-x-4 sm:inset-e-4 max-w-sm
tabs list     h-9 gap-1 rounded-xl p-1 bg-secondary text-muted-foreground; trigger px-3 py-1 text-sm rounded-md data-active:text-foreground; indicator rounded-md bg-card shadow-sm dark:bg-border; line: h-0.5 bg-foreground
toggle        h-9 min-w-9 px-3 rounded-lg text-sm hover:bg-accent aria-pressed:bg-accent aria-pressed:text-[tinted]; segmented group rounded-xl bg-secondary gap-1 p-1, items rounded-md, pressed bg-card shadow-sm dark:bg-border
sidebar       w-64 bg-sidebar border-e; group p-2; label h-8 px-3 text-xs font-semibold text-muted-foreground; menu gap-0.5; button h-9 gap-2 rounded-md px-3 py-2 text-base hover:bg-sidebar-accent data-active:bg-sidebar-accent data-active:[&_svg]:text-sidebar-primary
breadcrumb    list gap-3 text-sm text-muted-foreground; page text-foreground; separator [&>svg]:size-3 (CaretRight)
empty         gap-4 rounded-2xl border-dashed p-12 text-center; media icon size-10 rounded-lg bg-muted [&_svg]:size-5; title text-lg font-semibold; description text-sm/relaxed text-muted-foreground
alert         rounded-2xl p-5 text-base bg-muted grid gap-0.5 has-[>svg]:grid-cols-[auto_1fr] has-[>svg]:gap-x-3 *:[svg]:size-5 *:[svg]:text-primary (destructive: text-destructive); title font-semibold; description text-muted-foreground; action absolute top-4 inset-e-4
kbd           h-5.5 min-w-5.5 rounded-sm bg-muted px-1.5 text-xs text-muted-foreground max-md:hidden
skeleton      rounded-md bg-muted animate-pulse | spinner size-4 animate-[spin_0.8s_steps(8)_infinite] | progress track h-1 rounded-full bg-muted, indicator bg-primary
separator     bg-border h-px (vertical w-px)
```
