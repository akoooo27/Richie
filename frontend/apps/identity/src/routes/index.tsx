import { Button } from "@repo/ui/components/ui/button"
import { createFileRoute } from "@tanstack/react-router"

export const Route = createFileRoute("/")({ component: Home })

function Home() {
  return (
    <main className="flex min-h-svh flex-col items-start gap-4 p-8">
      <h1 className="text-4xl font-bold">Identity</h1>
      <Button>Continue</Button>
    </main>
  )
}
